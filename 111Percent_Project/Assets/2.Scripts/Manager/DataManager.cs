using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] public List<RoundData> RoundDataList = new List<RoundData>();

    public override void Awake()
    {
        base.Awake();

        RoundDataList = new List<RoundData>()
        {
            new RoundData(){ roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_05" , hp = 5, fallDownSpeed = 10f, jumpUpSpeed_PlayerCollision = 4f, jumpUpSpeed_PlayerDefense = 10f},
            new RoundData(){ roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_04" , hp = 6, fallDownSpeed = 11f, jumpUpSpeed_PlayerCollision = 3.5f, jumpUpSpeed_PlayerDefense = 9.5f },
            new RoundData(){ roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_03" , hp = 7, fallDownSpeed = 12f, jumpUpSpeed_PlayerCollision = 3f, jumpUpSpeed_PlayerDefense = 9f },
            new RoundData(){ roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" , hp = 8, fallDownSpeed = 13f, jumpUpSpeed_PlayerCollision = 2.5f, jumpUpSpeed_PlayerDefense = 8.5f },
            new RoundData(){ roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_02" , hp = 9, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
            new RoundData(){ roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_06" , hp = 9, fallDownSpeed = 14f, jumpUpSpeed_PlayerCollision = 2f, jumpUpSpeed_PlayerDefense = 8f },
            /*
            new RoundData(){ roundNumber = 6,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 7,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 8,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 9,  enemyType = 0, enemyPrefabName = "Enemy_Default" },
            new RoundData(){ roundNumber = 10, enemyType = 0, enemyPrefabName = "Enemy_Default" },
            */
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
