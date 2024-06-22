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
    private int abilityBuff_Missile = 0;


    private float laserAbilityCooltimeCounter = 0;
    private float missleAbilityCooltimeCounter = 0;

    private List<Effect_Bullet> missileSpawnedList = new List<Effect_Bullet>();

    private void FixedUpdate_Ability()
    {
        if (isDie)
            return;

        var enemyChild = InGameManager.Instance.CurrentEnemyChild();

        #region Laser
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
                    var additionalDmg = 0;
                    var upgradeData = DataManager.Instance.GetCurrentUpgradeData();
                    if (upgradeData != null)
                        additionalDmg += upgradeData.damageValue / 2;

                    laserScript.Setup(laserPoint, enemyChild.transform);

                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack + additionalDmg);
                    }, 0.1f, "laserAttack_1");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack + additionalDmg);
                    }, 0.7f, "laserAttack_2");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack + additionalDmg);
                    }, 1.3f, "laserAttack_3");
                    UtilityInvoker.Invoke(this, () =>
                    {
                        if (enemyChild != null)
                            enemyChild.GetHit(3 + 3 * abilityBuff_LaserAttack + additionalDmg);
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
        #endregion

        #region Missle
        if (abilityBuff_Missile > 0)
        {
            missilePoint.SafeSetActive(true);

            if (missleAbilityCooltimeCounter <= 0 && enemyChild != null)
            {
                missleAbilityCooltimeCounter = 0.3f;
                var pos = transform.position;
                var rot = Quaternion.LookRotation(Vector3.up);
                var bullet = InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_FireballMissileFire, pos, rot);
                if (bullet != null)
                {
                    bullet.TryGetComponent<Effect_Bullet>(out var bulletScript);
                    if (bulletScript)
                    {
                        bulletScript.Setup(missilePoint, enemyChild.transform);
                        missileSpawnedList.Add(bulletScript);
                    }
                }
            }
            else
            {
                missleAbilityCooltimeCounter -= Time.fixedDeltaTime;

                if (missleAbilityCooltimeCounter < 0)
                    missleAbilityCooltimeCounter = 0;
            }

            for (int i = missileSpawnedList.Count - 1; i >= 0; --i)
            {
                if (missileSpawnedList[i] == null)
                    continue;

                if (missileSpawnedList[i].gameObject.SafeIsActive() == false)
                {
                    var additionalDmg = 0;
                    var upgradeData = DataManager.Instance.GetCurrentUpgradeData();
                    if (upgradeData != null)
                        additionalDmg += upgradeData.damageValue / 2;

                    //지나간 bullet은 처리x
                    if (enemyChild != null && Vector3.Distance(missileSpawnedList[i].gameObject.transform.position, enemyChild.transform.position) < 5f)
                        enemyChild.GetHit(3 * abilityBuff_Missile + additionalDmg);

                    missileSpawnedList.Remove(missileSpawnedList[i]);
                }
            }
        }
        else
            missilePoint.SafeSetActive(false);
        #endregion
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
                    abilityBuff_IncreaseAttack += 0.1f; //10%증가
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
            case CommonDefine.Ability.Missile:
                {
                    abilityBuff_Missile += 1;
                }
                break;
        }
    }
}
