using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] public List<StageData> StageDataList = new List<StageData>();
    [SerializeField] public List<RoundData> RoundDataList = new List<RoundData>();
    [SerializeField] public List<AbilityData> AbilityDataList = new List<AbilityData>();

    public override void Awake()
    {
        base.Awake();
        StageDataList = new List<StageData>()
        {
            new StageData()
            {
                stageID = 0,
                stageNumber = 1,
                stageName = "GREEN",
                stageMapAssetName = "Map_SF_Bld_Preset_Elven_Castle_01",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 0, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_05" , hp = 20, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
                    new RoundData(){ stageID = 0, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 15, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
                    new RoundData(){ stageID = 0, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 20, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
                    new RoundData(){ stageID = 0, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 30, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
                    new RoundData(){ stageID = 0, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 40, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                    new RoundData(){ stageID = 0, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_01" , hp = 400, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                }
            },

            new StageData()
            {
                stageID = 1,
                stageNumber = 2,
                stageName = "WOODS",
                stageMapAssetName = "Map_SF_Bld_Preset_Elven_Castle_03",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 1, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_17" , hp = 10, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
                    new RoundData(){ stageID = 1, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 30, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
                    new RoundData(){ stageID = 1, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 40, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
                    new RoundData(){ stageID = 1, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 50, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
                    new RoundData(){ stageID = 1, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 60, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                    new RoundData(){ stageID = 1, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_02" , hp = 600, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                }
            },

            new StageData()
            {
                stageID = 2,
                stageNumber = 3,
                stageName = "GOLBLIN",
                stageMapAssetName = "Map_SF_Bld_Preset_Goblin_Castle_03",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 2, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_05" , hp = 250, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
                    new RoundData(){ stageID = 2, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 120, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
                    new RoundData(){ stageID = 2, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 100, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
                    new RoundData(){ stageID = 2, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 90, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
                    new RoundData(){ stageID = 2, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 100, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                    new RoundData(){ stageID = 2, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_03" , hp = 800, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                }
            },

            new StageData()
            {
                stageID = 3,
                stageNumber = 4,
                stageName = "KINGDOM",
                stageMapAssetName = "Map_SF_Bld_Preset_Human_Castle_01",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 3, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_19" , hp = 200, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
                    new RoundData(){ stageID = 3, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 120, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
                    new RoundData(){ stageID = 3, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 140, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
                    new RoundData(){ stageID = 3, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 160, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
                    new RoundData(){ stageID = 3, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 200, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                    new RoundData(){ stageID = 3, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_04" , hp = 1000, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                }
            },


            new StageData()
            {
                stageID = 4,
                stageNumber = 5,
                stageName = "FORTRESS",
                stageMapAssetName = "Map_SF_Bld_Preset_Human_Castle_02",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 4, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_18" , hp = 100, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
                    new RoundData(){ stageID = 4, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_19" , hp = 160, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
                    new RoundData(){ stageID = 4, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 180, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
                    new RoundData(){ stageID = 4, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 200, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
                    new RoundData(){ stageID = 4, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_15" , hp = 240, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                    new RoundData(){ stageID = 4, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_05" , hp = 1200, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
                }
            },

            new StageData()
            {
                stageID = 5,
                stageNumber = 6,
                stageName = "UNDEAD",
                stageMapAssetName = "Map_SF_Bld_Preset_Undead_Castle_03",
                RoundDataList = new List<RoundData>()
                {
                    new RoundData(){ stageID = 5, roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 120, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 8.5f},
                    new RoundData(){ stageID = 5, roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 130, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 7.5f },
                    new RoundData(){ stageID = 5, roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_15" , hp = 140, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 7f },
                    new RoundData(){ stageID = 5, roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_16" , hp = 150, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 6f },
                    new RoundData(){ stageID = 5, roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_17" , hp = 270, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 5f },
                    new RoundData(){ stageID = 5, roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Boss_06" , hp = 2000, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 4.5f },
                }
            },

        };

        AbilityDataList = new List<AbilityData>()
        {
            new AbilityData() {  id = 1, type = (int)CommonDefine.Ability.HealHp, name = "Heal Hp", desc = "Heals 1 Health Point" },
            new AbilityData() {  id = 2, type = (int)CommonDefine.Ability.IncreaseAttack, name = "Increase Attack", desc = "Increase Attack + 10%" },
            new AbilityData() {  id = 3, type = (int)CommonDefine.Ability.IncreaseSpeed, name = "Increase Speed", desc = "Increase Movement Speed + 5%" },
            new AbilityData() {  id = 4, type = (int)CommonDefine.Ability.DecreaseDefenseCooltime, name = "Decrease Defense Cooltime", desc = "Cooltime - 25%" },
            new AbilityData() {  id = 5, type = (int)CommonDefine.Ability.IncreaseAttackRange, name = "Increase Attack Range", desc = "Attack Range + 15%" },
            new AbilityData() {  id = 6, type = (int)CommonDefine.Ability.IncreaseDefenseRange, name = "Increase Defense Range", desc = "Defense Range + 10%" },
            new AbilityData() {  id = 7, type = (int)CommonDefine.Ability.Laser, name = "Laser", desc = "Laser Attack +20%" }
        };
    }


    #region Data

    [Serializable]
    public class StageData
    {
        public int stageID;
        public int stageNumber;
        public string stageName;
        public string stageMapAssetName;
        [SerializeField] public List<RoundData> RoundDataList = new List<RoundData>();
    }

    [Serializable]
    public class RoundData
    {
        public int stageID;
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
    public const string PLAYERPREFS_STAGE_KEY = "PLAYERPREFS_STAGE_KEY";
    public const string PLAYERPREFS_COIN_KEY = "PLAYERPREFS_COIN_KEY";
    public const string PLAYERPREFS_GEM_KEY = "PLAYERPREFS_GEM_KEY";
    public const string PLAYERPREFS_TUTORIAL_FINISH_KEY = "PLAYERPREFS_TUTORIAL_FINISH_KEY";

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

    public int GetSavedStageID()
    {
        if (PlayerPrefs.HasKey(PLAYERPREFS_STAGE_KEY))
        {
            return PlayerPrefs.GetInt(PLAYERPREFS_STAGE_KEY);
        }
        else
        {
            if (StageDataList != null && StageDataList.Count > 0)
            {
                PlayerPrefs.SetInt(PLAYERPREFS_STAGE_KEY, StageDataList[0].stageID);
                return StageDataList[0].stageID;
            }
            else
            {
                //Error...
                PlayerPrefs.SetInt(PLAYERPREFS_STAGE_KEY, 0);
                return 0;
            }
        }
    }

    public void SaveStageID(int id)
    {
        var stageData = StageDataList.Find(x => x.stageID.Equals(id));
        if (stageData != null)
        {
            PlayerPrefs.SetInt(PLAYERPREFS_STAGE_KEY, stageData.stageID);
        }
    }

    public StageData GetCurrentStageData()
    {
        var currID = GetSavedStageID();
        var data = StageDataList.Find(x => x.stageID.Equals(currID));
        return data;
    }

    public int Coin
    {
        get 
        {
            if (PlayerPrefs.HasKey(PLAYERPREFS_COIN_KEY))
                return PlayerPrefs.GetInt(PLAYERPREFS_COIN_KEY);
            else
            {
                PlayerPrefs.SetInt(PLAYERPREFS_COIN_KEY, 0);
                return PlayerPrefs.GetInt(PLAYERPREFS_COIN_KEY);
            }
        }

        set
        {
            PlayerPrefs.SetInt(PLAYERPREFS_COIN_KEY, value);
        }
    }

    public int Gem
    {
        get
        {
            if (PlayerPrefs.HasKey(PLAYERPREFS_GEM_KEY))
                return PlayerPrefs.GetInt(PLAYERPREFS_GEM_KEY);
            else
            {
                PlayerPrefs.SetInt(PLAYERPREFS_GEM_KEY, 0);
                return PlayerPrefs.GetInt(PLAYERPREFS_GEM_KEY);
            }
        }

        set
        {
            PlayerPrefs.SetInt(PLAYERPREFS_GEM_KEY, value);
        }
    }

    public int IsTutorialFinish
    {
        get
        {
            if (PlayerPrefs.HasKey(PLAYERPREFS_TUTORIAL_FINISH_KEY))
                return PlayerPrefs.GetInt(PLAYERPREFS_TUTORIAL_FINISH_KEY);
            else
            {
                PlayerPrefs.SetInt(PLAYERPREFS_TUTORIAL_FINISH_KEY, 0);
                return PlayerPrefs.GetInt(PLAYERPREFS_TUTORIAL_FINISH_KEY);
            }
        }

        set
        {
            PlayerPrefs.SetInt(PLAYERPREFS_TUTORIAL_FINISH_KEY, value);
        }
    }

    #endregion
}
