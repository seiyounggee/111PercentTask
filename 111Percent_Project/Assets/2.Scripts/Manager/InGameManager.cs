using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class InGameManager : MonoSingleton<InGameManager>
{
    [ReadOnly] public Player player = null;

    [ReadOnly] public int currentRoundIndex = -1;
    [ReadOnly] public int lastRoundIndex = -1;

    private List<Enemy> enemyList = new List<Enemy>();
    public Enemy currentEnemy = null;

    public CommonDefine.InGameState CurrentInGameState = CommonDefine.InGameState.None;
    public CommonDefine.GameEndType CurrentGameEndType = CommonDefine.GameEndType.None;

    public long GameScore { get; set; } = 0;

    public void StartInGame()
    {
        UtilityCoroutine.StartCoroutine(ref gameRoutine, GameRoutine(), this);
    }

    private IEnumerator gameRoutine = null;
    private IEnumerator GameRoutine()
    {
        Clear();

        SetGameState(CommonDefine.InGameState.Waiting);
        SetupInGameUI();
        SetMap();
        SetPlayer();
        SetCamera();
        //SetInput();
        SetPoolGameObjects();

        SetGameState(CommonDefine.InGameState.Play);
        yield return StartCoroutine(EnemyRound());

        SetupInGameUI_Result();
        SetGameState(CommonDefine.InGameState.End);
        EndGame();

        yield break;
    }

    private void Clear()
    {
        CurrentInGameState = CommonDefine.InGameState.None;
        CurrentGameEndType = CommonDefine.GameEndType.None;

        GameScore = 0;

        currentRoundIndex = -1;
        lastRoundIndex = -1;

        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }

        if (enemyList != null && enemyList.Count > 0)
        {
            foreach (var i in enemyList)
            {
                if (i != null)
                    Destroy(i.gameObject);
            }
            enemyList.Clear();
        }
    }

    private void SetupInGameUI()
    {
        UIManager.Instance.HideGrouped_Outgame();

        var inGameUI = PrefabManager.Instance.UI_InGame;
        UIManager.Instance.ShowUI(inGameUI);
    }

    private void SetupInGameUI_Result()
    {
        var inGameUI = PrefabManager.Instance.UI_InGame;
        switch (CurrentGameEndType)
        {
            case CommonDefine.GameEndType.GameOver:
                {
                    inGameUI.ActivateGameOver();
                }
                break;

            case CommonDefine.GameEndType.GameClear:
                {
                    inGameUI.ActivateGameClear();
                }
                break;
        }
    }

    private void SetMap()
    {
        var list = PrefabManager.Instance.MapPrefabList;
        var randomMap = list[UnityEngine.Random.Range(0, list.Count)];
        var go = GameObject.Instantiate(randomMap);
        go.SafeSetActive(true);
    }

    private void SetPlayer()
    {
        var go = GameObject.Instantiate(PrefabManager.Instance.PlayerPrefab, Vector3.zero, Quaternion.identity);
        var _player = go.GetComponent<Player>();
        if (_player != null)
        {
            player = _player;
            player.Setup(Player.Type.IngamePlayer);
        }
    }

    private void SetGameState(CommonDefine.InGameState state)
    {
        CurrentInGameState = state;

#if UNITY_EDITOR
        Debug.Log("<color=white>InGameState >>> " + CurrentInGameState + "</color>");
#endif
    }

    private IEnumerator EnemyRound()
    {
        var roundList = DataManager.Instance.RoundDataList;
        if (roundList == null || roundList.Count <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>Error...! No Round Data Existing</color>");
#endif
            yield break;
        }

        var prefabList = PrefabManager.Instance.EnemyPrefabList;
        if (prefabList == null || prefabList.Count <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>Error...! No Prefab List Existing</color>");
#endif
            yield break;
        }

        roundList.Sort((x, y) => x.roundNumber.CompareTo(y.roundNumber));

        //만들어주자...
        for (int i = 0; i < roundList.Count; i++)
        {
            var prefab = prefabList.Find(x => x.gameObject.name.Equals(roundList[i].enemyPrefabName));
            if (prefab != null)
            {
                var go = GameObject.Instantiate(prefab, new Vector3(0f, 50f, 0f), Quaternion.identity);
                var enemy = go.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Hide();
                    enemy.SetData(roundList[i]);
                    enemyList.Add(enemy);
                }
            }
        }

        if (enemyList == null || enemyList.Count <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>Error...! No enemyList Existing</color>");
#endif
            yield break;
        }

        //최초 적 세팅
        currentRoundIndex = 0;
        currentEnemy = enemyList[currentRoundIndex];
        lastRoundIndex = enemyList.Count - 1;
        currentEnemy.Show();

        while (true)
        {
            //내가 죽은 경우 GameOver
            if (player != null && player.isDie)
            {
                CurrentGameEndType = CommonDefine.GameEndType.GameOver;
                break;
            }

            //적이 죽은 경우...
            if (currentEnemy.IsDead())
            {
                currentEnemy.Hide();

                if (currentRoundIndex == lastRoundIndex)
                {
                    //마지막 놈이 죽은 경우 Loop 빠져나가자
                    CurrentGameEndType = CommonDefine.GameEndType.GameClear;
                    break;
                }

                yield return new WaitForSeconds(2f); //2초후에 생성해주자

                //다음꺼 보여주자
                ++currentRoundIndex;
                currentEnemy = enemyList[currentRoundIndex];
                currentEnemy.Show();

            }

            yield return null;
        }
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        Debug.Log("<color=cyan>EndGame()</color>");
#endif
    }

    public void AddScore(int score)
    {
        if (GameScore + score < long.MaxValue)
        {
            GameScore += score;

            PrefabManager.Instance.UI_InGame.UpdateScore(GameScore.ToString());
        }
    }
}
