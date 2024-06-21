using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private List<DataManager.AbilityData> abilityObtained = new List<DataManager.AbilityData>();

    private float abilityBuff_IncreaseAttack = 0f;
    private float abilityBuff_IncreaseSpeed = 0f;
    private float abilityBuff_DecreaseDefenseCooltime = 0f;
    private float abilityBuff_IncreaseAttackRange = 0f;
    private float abilityBuff_IncreaseDefenseRange = 0f;
    private int abilityBuff_LaserAttack = 0;


    private float laserAbilityCooltimeCounter = 0;


    private void FixedUpdate_Ability()
    {
        if (isDie)
            return;

        var enemyChild = InGameManager.Instance.CurrentEnemyChild();
        if (abilityBuff_LaserAttack > 0)
        {
            laserPoint.SafeSetActive(true);

            if (laserAbilityCooltimeCounter <= 0 && enemyChild != null)
            {
                laserAbilityCooltimeCounter = 3f;
                var pos = transform.position;
                var rot = Quaternion.Euler(-90f, 0f, 0f);
                var laser = InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_LaserBeamRed, pos, rot, 1f, this.transform);
                laser.TryGetComponent<Effect_Laser>(out var laserScript);
                if (laserScript != null)
                {
                    laserScript.Setup(laserPoint, enemyChild.transform);

                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack);
                    }, 0.1f, "laserAttack_1");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack);
                    }, 0.7f, "laserAttack_2");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack);
                    }, 1.3f, "laserAttack_3");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack);
                    }, 2f, "laserAttack_4");
                }
            }
            else
            {
                laserAbilityCooltimeCounter -= Time.fixedDeltaTime;

                if (laserAbilityCooltimeCounter < 0)
                    laserAbilityCooltimeCounter = 0;
            }
        }
        else
            laserPoint.SafeSetActive(false);
    }

    public void GetAbility(DataManager.AbilityData abilityData)
    {
        abilityObtained.Add(abilityData);

        switch ((CommonDefine.Ability)abilityData.type)
        {
            case CommonDefine.Ability.HealHp:
                {
                    if (currHealth > 0 && currHealth < maxHealth)
                    {
                        ++currHealth;
                        PrefabManager.Instance.UI_InGame.UpdateHealthObj();
                    }
                }
                break;
            case CommonDefine.Ability.IncreaseAttack:
                {
                    abilityBuff_IncreaseAttack += 0.1f; //10%Áõ°¡
                }
                break;
            case CommonDefine.Ability.IncreaseSpeed:
                {
                    abilityBuff_IncreaseSpeed += 0.05f;
                }
                break;
            case CommonDefine.Ability.DecreaseDefenseCooltime:
                {
                    abilityBuff_DecreaseDefenseCooltime += 0.2f;

                    defenseInputCooltime = 3 - abilityBuff_DecreaseDefenseCooltime;

                    defenseInputCooltime = Mathf.Clamp(defenseInputCooltime, 2f, 3f);
                }
                break;
            case CommonDefine.Ability.IncreaseAttackRange:
                {
                    abilityBuff_IncreaseAttackRange += 0.15f;

                    attackTrigger.transform.localScale = (1 + abilityBuff_IncreaseAttackRange) * Vector3.one;
                }
                break;
            case CommonDefine.Ability.IncreaseDefenseRange:
                {
                    abilityBuff_IncreaseDefenseRange += 0.1f;

                    defenseTrigger.transform.localScale = (1 + abilityBuff_IncreaseDefenseRange) * Vector3.one;
                }
                break;
            case CommonDefine.Ability.Laser:
                {
                    abilityBuff_LaserAttack += 1;
                }
                break;
        }
    }
}
