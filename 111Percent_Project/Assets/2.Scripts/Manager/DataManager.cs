using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] public List<RoundData> RoundDataList = new List<RoundData>();
    [SerializeField] public List<AbilityData> AbilityDataList = new List<AbilityData>();

    public override void Awake()
    {
        base.Awake();

        RoundDataList = new List<RoundData>()
        {
            new RoundData(){ roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_05" , hp = 50, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
            new RoundData(){ roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 60, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
            new RoundData(){ roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 70, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
            new RoundData(){ roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 80, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
            new RoundData(){ roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 90, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
            new RoundData(){ roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_06" , hp = 90, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
            /*
            new RoundData(){ roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 7,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 8,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 9,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 10, enemyType = 0, enemyPrefabName = "Enemy_Default" },
            */
        };

        AbilityDataList = new List<AbilityData>()
        {
            new AbilityData() {  id = 1, type = (int)CommonDefine.Ability.HealHp, name = "Heal Hp", desc = "Heals 1 Health Point" },
            new AbilityData() {  id = 2, type = (int)CommonDefine.Ability.IncreaseAttack, name = "Increase Attack", desc = "Increase Attack + 5" },
            new AbilityData() {  id = 3, type = (int)CommonDefine.Ability.IncreaseSpeed, name = "Increase Speed", desc = "Increase Speed + 3" },
            new AbilityData() {  id = 4, type = (int)CommonDefine.Ability.DecreaseDefenseCooltime, name = "Decrease Defense Cooltime", desc = "Cooltime - 25%" },
            new AbilityData() {  id = 5, type = (int)CommonDefine.Ability.IncreaseAttackRange, name = "Increase Attack Range", desc = "Attack Range + 20%" },
            new AbilityData() {  id = 6, type = (int)CommonDefine.Ability.IncreaseDefenseRange, name = "Increase Defense Range", desc = "Defense Range + 10%" },
        };
    }


    #region Data

    [Serializable]
    public class RoundData
    {
        public int roundNumber;
        public int enemyType;
        public string enemyPrefabName;
        public int hp;
        public float fallDownSpeed;
        public float jumpUpSpeed_PlayerCollision;
        public float jumpUpSpeed_PlayerDefense;
    }

    [Serializable]
    public class AbilityData
    {
        public int id;
        public int type;
        public string name;
        public string desc;
    }    

    #endregion

    #region PlayerPrefs

    public const string PLAYERPREFS_SKIN_NAME_KEY = "PLAYER_SKIN_NAME_KEY";

    public GameObject GetSavedSkinPrefab()
    {
        GameObject go = null;
        var list = PrefabManager.Instance.PlayerSkinPrefabList;
        if (PlayerPrefs.HasKey(PLAYERPREFS_SKIN_NAME_KEY))
        {
            var name = PlayerPrefs.GetString(PLAYERPREFS_SKIN_NAME_KEY);
            go = list.Find(x => x.name.Equals(name));
        }
        else
        {
            if (list != null && list.Count > 0)
            {
                go = list[0];
                PlayerPrefs.SetString(PLAYERPREFS_SKIN_NAME_KEY, go.name);
            }
        }

        return go;
    }

    public void SaveSkinPrefab(string name)
    {
        var list = PrefabManager.Instance.PlayerSkinPrefabList;
        var go = list.Find(x => x.name.Equals(name));
        if (go != null)
        {
            PlayerPrefs.SetString(PLAYERPREFS_SKIN_NAME_KEY, go.name);
        }
    }

    #endregion
}
