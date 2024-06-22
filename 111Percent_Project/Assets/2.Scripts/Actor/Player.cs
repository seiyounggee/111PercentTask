using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Agent
{
    [SerializeField] Animator anim = null;
    [SerializeField] Trigger_Callback bodyTrigger = null;
    [SerializeField] Trigger_Callback attackTrigger = null;
    [SerializeField] Trigger_Callback defenseTrigger = null;
    [SerializeField] Transform laserPoint = null;
    [SerializeField] Transform missilePoint = null;

    private Transform rightHand = null;
    private Transform leftHand = null;

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

    public int CurrentCombo { get; set; }

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

        CurrentCombo = 0;

        abilityObtained.Clear();

        abilityBuff_IncreaseAttack = 0f;
        abilityBuff_IncreaseSpeed = 0f;
        abilityBuff_DecreaseDefenseCooltime = 0f;
        abilityBuff_IncreaseAttackRange = 0f;
        abilityBuff_IncreaseDefenseRange = 0f;
        abilityBuff_LaserAttack = 0;
        abilityBuff_Missile = 0;

        missileSpawnedList.Clear();
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
        var selectedIndex = 0;
        if (DataManager.Instance.CurrentWeaponSkinIndex < list_weapon.Count)
            selectedIndex = DataManager.Instance.CurrentWeaponSkinIndex;
        else
            selectedIndex = UnityEngine.Random.Range(0, list_weapon.Count);

        var weapon = GameObject.Instantiate(list_weapon[selectedIndex], Vector3.zero, Quaternion.identity);
        weapon.transform.SetParent(rightHand);
        weapon.transform.localPosition = new Vector3(0.08f, -0.011f, -0.4f);
        weapon.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void Jump()
    {
            if (isDie)
            return; //이미 죽었으면... 

        if (isGrounded && Mathf.Approximately(jumpUpVelocity.y, 0f))
        {
            jumpUpVelocity = Vector3.up * 25f;
            jumpUpVelocity += abilityBuff_IncreaseSpeed * Vector3.up * 25f;
            fallDownVelocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (isDie)
            return; //이미 죽었으면... 

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

        SoundManager.Instance.PlaySound(SoundManager.SoundClip.Ingame_Attack);
    }

    public void Defense()
    {
        if (isDie)
            return; //이미 죽었으면... 

        if (defenseTrigger.SafeIsActive())
            return;

        if (defenseInputCooltimeCounter > 0f && defenseTrigger.SafeIsActive() == false)
        {
            //쿨타임중...
            PrefabManager.Instance.UI_InGame.ActivateMessageUI("Cooldown in progress, cannot be used");

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
            FixedUpdate_Movement();
            FixedUpdate_Ability();
        }
    }

    private void FixedUpdate_Movement()
    {
        if (jumpUpVelocity.y > 0)
        {
            this.transform.position += Vector3.up * Time.fixedDeltaTime * jumpUpVelocity.y;

            jumpUpVelocity -= Vector3.up * Time.fixedDeltaTime * 10f;
            if (jumpUpVelocity.y <= 0)
            {
                jumpUpVelocity.y = 0;
            }
        }

        if (isGrounded == false && jumpUpVelocity.y <= 0)
        {
            this.transform.position += Vector3.up * Time.fixedDeltaTime * fallDownVelocity.y;
            fallDownVelocity -= Vector3.up * Time.fixedDeltaTime * 20f;

            //바닥 뚫고 못가게...
            if (this.transform.position.y <= 0f)
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }

        //적 뚤고 올라가지 않게 방지...
        if (isGrounded == false)
        {
            var enemyChild = InGameManager.Instance.CurrentEnemyChild();
            if (enemyChild != null)
            {
                if (enemyChild.transform.position.y < this.transform.position.y)
                {
                    jumpUpVelocity.y = 0;
                }
            }
        }

        if (defenseInputCooltimeCounter > 0)
            defenseInputCooltimeCounter -= Time.fixedDeltaTime;
    }

    public void GetHit()
    {
        if (isDie)
            return; //이미 죽었으면... 

        //hp 관련... 처리

        --currHealth;

        if (currHealth <= 0)
        {
            currHealth = 0;
            SetDie();
        }

        defenseInputCooltimeCounter = 0; //맞았으면 방어 쿨타임 초기화
        CurrentCombo = 0;
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
                var damage = 100;
                damage += (int)((float)damage * abilityBuff_IncreaseAttack);
                damage += Random.Range(0, 30);
                var upgradeData = DataManager.Instance.GetCurrentUpgradeData();
                if (upgradeData != null)
                    damage += upgradeData.damageValue;
                damage += DataManager.Instance.CurrentWeaponSkinIndex * 20;
#if UNITY_EDITOR
                damage += cheatDmg;
#endif
                enemyChild.GetHit(damage);

                Vector3 pos = other.ClosestPoint(transform.position);
                var randomX = UnityEngine.Random.Range(-3f, 3f);
                pos.x += randomX;
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_Hit, pos, Quaternion.identity);
                InGameManager.Instance.ShakeCamera(5f, 0.2f);

                var dmgTxt = InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_DamageText, pos, Quaternion.identity);
                if (dmgTxt != null)
                {
                    dmgTxt.TryGetComponent<Effect_DamageText>(out var dmgScript);
                    if (dmgScript != null)
                    {
                        var randomVel = new Vector3(
                                        Random.Range(0.1f, 2f) * (Random.value < 0.5f ? -1 : 1),
                                        Random.Range(5f, 10f),
                                        0f
                        );
                        dmgScript.Setup(pos, randomVel, damage.ToString());
                    }
                }

                ++CurrentCombo;
                PrefabManager.Instance.UI_InGame.UpdateCombo("Combo x" + CurrentCombo.ToString());
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
                defenseTrigger.SafeSetActive(false);

                var hitPoint = other.ClosestPoint(transform.position);
                var dist = Mathf.Abs(transform.position.y - hitPoint.y);
                //보통 dist 값은 2.3 ~ 4
                var strength_percentage = (4.5f - dist) / 100f * 15f;
                strength_percentage += Random.Range(0f, 0.2f); //랜덤으로 버프...?
                strength_percentage = Mathf.Clamp(strength_percentage, 0, 1);

                enemyChild.GetBlocked(strength_percentage);

                //올라가고 있는 경우 였으면 아래로 밀어주자(?)
                if (jumpUpVelocity.y > 0)
                {
                    jumpUpVelocity = Vector3.zero;
                    fallDownVelocity -= Vector3.up * Time.fixedDeltaTime * 25f;
                }

                defenseInputCooltimeCounter = defenseInputCooltime / 4f; //성공한 경우 시간 단축

                Vector3 pos = other.ClosestPoint(transform.position);
                pos.z = 3f;
                InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_KaPow, pos, Quaternion.identity);

                ++CurrentCombo;
                PrefabManager.Instance.UI_InGame.UpdateCombo("Combo x" + CurrentCombo.ToString());

                SoundManager.Instance.PlaySound(SoundManager.SoundClip.Ingame_Defense);
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

            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (cheatDmg != 0)
                    cheatDmg = 0;
                else
                    cheatDmg = 1000;
            }
        }
    }
#endif

    private int cheatDmg = 0;
}
