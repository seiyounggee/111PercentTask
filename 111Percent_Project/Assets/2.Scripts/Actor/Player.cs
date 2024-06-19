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

    private float speed = 10f;
    [ReadOnly] public bool isGrounded = false;

    private Vector3 jumpUpVelocity = Vector3.zero;
    private Vector3 fallDownVelocity = Vector3.zero;


    private const string ANIM_STATE_IDLE = "isIdle";
    private const string ANIM_STATE_JUMPING = "isJump";
    private const string ANIM_STATE_ATTACK = "isAttack";
    private const string ANIM_STATE_DEFENSE = "isDefense";
    private const string ANIM_STATE_DIE = "isDie";

    private const string ANIM_CLIP_ATTACK_NAME = "Attack";
    private const string ANIM_CLIP_DEFENCE_NAME = "Defense";

    private const int ANIM_LAYER_BASE = 0;
    private const int ANIM_LAYER_OVERRIDE = 1;

    private float defenseInputCooltimeCounter = 0f;
    private float defenseInputCooltime = 3f;

    [ReadOnly] public int currHealth = 3;
    [ReadOnly] public int maxHealth = 3;

    [ReadOnly] public bool isDie = false;

    private int currentCombo = 0;

    public enum Type { None, IngamePlayer, OutgamePlayer}
    public Type CurrentType { get; set; }

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

    public void Setup(Type type)
    {
        Clear();

        CurrentType = type;
        SetSkin(DataManager.Instance.GetSavedSkinPrefab());
    }

    private void Clear()
    {
        attackTrigger.SafeSetActive(false);
        defenseTrigger.SafeSetActive(false);

        maxHealth = 3;
        currHealth = maxHealth;

        isDie = false;

        currentCombo = 0;
    }

    public void SetSkin(GameObject skin = null)
    {
        var list = PrefabManager.Instance.PlayerSkinPrefabList;

        GameObject prefab = null;
        var skinIndex = 0;
        if (skin == null)
        {
            skinIndex = UnityEngine.Random.Range(0, list.Count);
            prefab = list[skinIndex];
        }
        else
        {
            prefab = skin;
        }

        var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
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
        if (isDie)
            return; //�̹� �׾�����... 

        jumpUpVelocity = Vector3.up * 25f;
        fallDownVelocity = Vector3.zero;
    }

    public void Attack()
    {
        if (isDie)
            return; //�̹� �׾�����... 

        anim.SetTrigger(ANIM_STATE_ATTACK);
        attackTrigger.SafeSetActive(false);
        attackTrigger.SafeSetActive(true);

        UtilityInvoker.Invoke(this, () =>
        {
            attackTrigger.SafeSetActive(false);
        }, 0.5f, "attack");

        var pos = this.transform.position + Vector3.up * 2f;
        pos.z = 3f;
        var rot = Quaternion.Euler(180f, 360f, 30f);
        InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_SwordSlashThickRed, pos, rot);
    }

    public void Defense()
    {
        if (isDie)
            return; //�̹� �׾�����... 

        if (defenseInputCooltimeCounter > 0f)
        {
            //��Ÿ����...

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

    public void SetDie()
    {
        isDie = true;

        anim.SetTrigger(ANIM_STATE_DIE);
    }

    private void Update()
    {
        if (anim != null)
        {
            if (CurrentType == Type.IngamePlayer)
            {
                if (isDie == false)
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
                else
                {
                    anim.SetBool(ANIM_STATE_IDLE, false);
                    anim.SetBool(ANIM_STATE_JUMPING, false);
                }
            }
            else if (CurrentType == Type.OutgamePlayer)
            {
                anim.SetBool(ANIM_STATE_IDLE, true);
                anim.SetBool(ANIM_STATE_JUMPING, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (CurrentType == Type.IngamePlayer)
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

                //�ٴ� �հ� ������...
                if (this.transform.position.y <= 0f)
                    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            }

            if (defenseInputCooltimeCounter > 0)
                defenseInputCooltimeCounter -= Time.fixedDeltaTime;
        }
    }

    public void GetHit()
    {
        if (isDie)
            return; //�̹� �׾�����... 

        //hp ����... ó��

        --currHealth;

        if (currHealth <= 0)
        {
            currHealth = 0;
            SetDie();
        }

        currentCombo = 0;
        PrefabManager.Instance.UI_InGame.UpdateCombo(string.Empty);

        PrefabManager.Instance.UI_InGame.UpdateHealthObj();
        PrefabManager.Instance.UI_InGame.ActivateHitUI();
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
                enemyChild.GetHit(10);

                Vector3 pos = other.ClosestPoint(transform.position);
                var randomX = UnityEngine.Random.Range(-3f, 3f);
                pos.x += randomX;
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_Hit, pos, Quaternion.identity);
                InGameManager.Instance.ShakeCamera(5f, 0.2f);

                ++currentCombo;
                PrefabManager.Instance.UI_InGame.UpdateCombo("Combo x" + currentCombo.ToString());
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

                //�ö󰡰� �ִ� ��� ������ �Ʒ��� �о�����(?)
                if (jumpUpVelocity.y > 0)
                {
                    jumpUpVelocity = Vector3.zero;
                    fallDownVelocity -= Vector3.up * Time.fixedDeltaTime * 20f;
                }

                defenseInputCooltimeCounter = defenseInputCooltime / 2f; //������ ��� �ð� ����

                Vector3 pos = other.ClosestPoint(transform.position);
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_KaPow, pos, Quaternion.identity);

                ++currentCombo;
                PrefabManager.Instance.UI_InGame.UpdateCombo("Combo x" + currentCombo.ToString());
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

    public bool IsDefenceActive()
    {
        return defenseTrigger.SafeIsActive();
    }



#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (CurrentType == Type.IngamePlayer)
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
    }
#endif
}
