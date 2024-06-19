using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{
    [SerializeField] Animator anim = null;
    [SerializeField] Trigger_Callback bodyTrigger = null;
    [SerializeField] Trigger_Callback attackTrigger = null;
    [SerializeField] Trigger_Callback defenseTrigger = null;

    private Transform rightHand = null;
    private Transform leftHand = null;

    public float speed = 10f;
    [ReadOnly] public bool isGrounded = false;

    private Vector3 jumpUpVelocity = Vector3.zero;
    private Vector3 fallDownVelocity = Vector3.zero;


    private const string ANIM_STATE_IDLE = "isIdle";
    private const string ANIM_STATE_JUMPING = "isJump";
    private const string ANIM_STATE_ATTACK = "isAttack";
    private const string ANIM_STATE_DEFENSE = "isDefense";

    private const string ANIM_CLIP_ATTACK_NAME = "Attack";
    private const string ANIM_CLIP_DEFENCE_NAME = "Defense";

    private const int ANIM_LAYER_BASE = 0;
    private const int ANIM_LAYER_OVERRIDE = 1;

    private float defenseInputCooltimeCounter = 0f;
    private float defenseInputCooltime = 3f;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        bodyTrigger.OnTriggerEnterAction += OnTriggerEnterAction_Body;
        bodyTrigger.OnTriggerExitAction += OnTriggerExitAction_Body;

        attackTrigger.OnTriggerEnterAction += OnTriggerEnterAction_Attack;
        attackTrigger.OnTriggerExitAction += OnTriggerExitAction_Attack;

        defenseTrigger.OnTriggerEnterAction += OnTriggerEnterAction_Defense;
        defenseTrigger.OnTriggerExitAction += OnTriggerExitAction_Defense;
    }

    private void OnDisable()
    {
        bodyTrigger.OnTriggerEnterAction -= OnTriggerEnterAction_Body;
        bodyTrigger.OnTriggerExitAction -= OnTriggerExitAction_Body;

        attackTrigger.OnTriggerEnterAction -= OnTriggerEnterAction_Attack;
        attackTrigger.OnTriggerExitAction -= OnTriggerExitAction_Attack;

        defenseTrigger.OnTriggerEnterAction -= OnTriggerEnterAction_Defense;
        defenseTrigger.OnTriggerExitAction -= OnTriggerExitAction_Defense;
    }

    public void Setup()
    {
        Clear();
        SetSkin();
    }

    private void Clear()
    {
        attackTrigger.SafeSetActive(false);
        defenseTrigger.SafeSetActive(false);
    }

    private void SetSkin()
    {
        var list = PrefabManager.Instance.PlayerSkinPrefabList;
        var randomSkin = list[UnityEngine.Random.Range(0, list.Count)];
        var go = GameObject.Instantiate(randomSkin, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(this.transform);
        anim = go.GetComponent<Animator>();

        var childTrans = go.transform.GetComponentsInChildren<Transform>();
        foreach (var i in childTrans)
        {
            if (i.gameObject.name.Equals("Hand_Right_jnt"))
                rightHand = i;

            if (i.gameObject.name.Equals("Left_Right_jnt"))
                leftHand = i;
        }

        var list_weapon = PrefabManager.Instance.WeaponSkinPrefabList;
        var randomSkin_weapon = list_weapon[UnityEngine.Random.Range(0, list_weapon.Count)];

        var weapon = GameObject.Instantiate(randomSkin_weapon, Vector3.zero, Quaternion.identity);
        weapon.transform.SetParent(rightHand);
        weapon.transform.localPosition = new Vector3(0.08f, -0.011f, -0.4f);
        weapon.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void Jump()
    {
        jumpUpVelocity = Vector3.up * 25f;
        fallDownVelocity = Vector3.zero;
    }

    public void Attack()
    {
        anim.SetTrigger(ANIM_STATE_ATTACK);
        attackTrigger.SafeSetActive(false);
        attackTrigger.SafeSetActive(true);

        UtilityInvoker.Invoke(this, () =>
        {
            attackTrigger.SafeSetActive(false);
        }, 0.5f, "attack");
    }

    public void Defense()
    {
        if (defenseInputCooltimeCounter > 0f)
        {
            //쿨타임중...

            return;
        }

        anim.SetTrigger(ANIM_STATE_DEFENSE);
        defenseTrigger.SafeSetActive(false);
        defenseTrigger.SafeSetActive(true);

        defenseInputCooltimeCounter = defenseInputCooltime;

        UtilityInvoker.Invoke(this, () =>
        {
            defenseTrigger.SafeSetActive(false);
        }, 0.5f, "defense");
    }

    private void Update()
    {
        if (anim != null)
        {
            if (isGrounded)
            {
                anim.SetBool(ANIM_STATE_IDLE, true);
                anim.SetBool(ANIM_STATE_JUMPING, false);
            }
            else
            {
                anim.SetBool(ANIM_STATE_IDLE, false);

                if (IsAnimationPlaying(ANIM_CLIP_ATTACK_NAME, ANIM_LAYER_OVERRIDE) || IsAnimationPlaying(ANIM_CLIP_DEFENCE_NAME, ANIM_LAYER_OVERRIDE))
                {
                    anim.SetBool(ANIM_STATE_JUMPING, false);
                    anim.SetBool(ANIM_STATE_IDLE, true);
                }
                else
                {
                    anim.SetBool(ANIM_STATE_JUMPING, true);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (jumpUpVelocity.y > 0)
        {
            this.transform.position += Vector3.up * Time.fixedDeltaTime * jumpUpVelocity.y;

            jumpUpVelocity -= Vector3.up * Time.fixedDeltaTime * speed;
            if (jumpUpVelocity.y <= 0)
            { 
                jumpUpVelocity.y = 0;
            }
        }

        if (isGrounded == false && jumpUpVelocity.y <= 0)
        {
            this.transform.position += Vector3.up * Time.fixedDeltaTime * fallDownVelocity.y;
            fallDownVelocity -= Vector3.up * Time.fixedDeltaTime * speed;

            //바닥 뚫고 못가게...
            if (this.transform.position.y <= 0f)
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }

        if (defenseInputCooltimeCounter > 0)
            defenseInputCooltimeCounter -= Time.fixedDeltaTime;
    }

    private void OnTriggerEnterAction_Body(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = true;
        }

        if (other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            jumpUpVelocity.y = 0;
            fallDownVelocity = Vector3.down * Time.deltaTime * 15f;
        }
    }

    private void OnTriggerExitAction_Body(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnterAction_Attack(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            var enemyChild = other.transform.GetComponent<Enemy_Child>();
            if (enemyChild != null)
            {
                //Debug.Log("Hit Enemy!!");
                enemyChild.GetHit(1);

                Vector3 pos = other.ClosestPoint(transform.position);
                var randomX = UnityEngine.Random.Range(-3f, 3f);
                pos.x += randomX;
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_Hit, pos, Quaternion.identity);
                InGameManager.Instance.ShakeCamera(5f, 0.2f);
            }
        }
    }

    private void OnTriggerExitAction_Attack(Collider other)
    {

    }

    private void OnTriggerEnterAction_Defense(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            var enemyChild = other.transform.GetComponent<Enemy_Child>();
            if (enemyChild != null)
            {
                //Debug.Log("Defense Success");
                enemyChild.GetBlocked();

                defenseInputCooltimeCounter = defenseInputCooltime / 2f; //성공한 경우 시간 단축

                Vector3 pos = other.ClosestPoint(transform.position);
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_KaPow, pos, Quaternion.identity);
            }
        }
    }

    private void OnTriggerExitAction_Defense(Collider other)
    {

    }


    bool IsAnimationPlaying(string stateName, int layer = 0)
    {
        if (anim == null)
            return false;

        // Check the current state in the specified layer (0 is the default layer)
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(layer);

        // Check if the current state's name matches the provided stateName
        // Note: stateName should match the name in the Animator controller
        return stateInfo.IsName(stateName);
    }




#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Attack();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            if (isGrounded && Mathf.Approximately(jumpUpVelocity.y, 0f))
                Jump();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            Defense();
        }
    }
#endif
}
