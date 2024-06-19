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
            new RoundData(){ roundNumber = 1,  enemyType = 0, enemyPrefabName = "Enemy_01" },
            new RoundData(){ roundNumber = 2,  enemyType = 0, enemyPrefabName = "Enemy_01" },
            new RoundData(){ roundNumber = 3,  enemyType = 0, enemyPrefabName = "Enemy_01" },
            new RoundData(){ roundNumber = 4,  enemyType = 0, enemyPrefabName = "Enemy_01" },
            new RoundData(){ roundNumber = 5,  enemyType = 0, enemyPrefabName = "Enemy_01" },
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
    }

    #endregion
}
