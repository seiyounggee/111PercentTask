using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class InGameManager : MonoSingleton<InGameManager>
{
    [ReadOnly] public Player player = null;

    public int CurrentRoundIndex { get; set; } = -1;
    public int LastRoundIndex { get; set; } = -1;

    public List<Enemy> enemyList = new List<Enemy>();
    public Enemy currentEnemy = null;

    public CommonDefine.InGameState CurrentInGameState = CommonDefine.InGameState.None;
    public CommonDefine.GameEndType CurrentGameEndType = CommonDefine.GameEndType.None;

    public int GameScore { get; set; } = 0;

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
        yield return StartCoroutine(Tutorial());
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

        CurrentRoundIndex = -1;
        LastRoundIndex = -1;

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
        var stageData = DataManager.Instance.GetCurrentStageData();
        if (stageData != null)
        {
            var map = list.Find(x => x.name.Equals(stageData.stageMapAssetName));
            if (map != null)
            {
                var go = GameObject.Instantiate(map);
                go.SafeSetActive(true);
            }
        }
        else
        {
            var randomMap = list[UnityEngine.Random.Range(0, list.Count)];
            var go = GameObject.Instantiate(randomMap);
            go.SafeSetActive(true);
        }
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

    private IEnumerator Tutorial()
    {
        if (DataManager.Instance.IsTutorialFinish == 0)
        {
            PrefabManager.Instance.UI_InGame.ActivateTutorialUI();
        }
        else
        {
            //튜토리얼 이미 끝난 상태
            yield break;
        }

        while (true)
        {
            yield return null;

            if (DataManager.Instance.IsTutorialFinish == 1)
                break;
        }
    }


    private IEnumerator EnemyRound()
    {
        var stageList = DataManager.Instance.StageDataList;
        var stageID = DataManager.Instance.GetSavedStageID();
        var stage = stageList.Find(x => x.stageID.Equals(stageID));
        if (stage == null)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>Error...! No Stage Data Existing</color>");
#endif
            yield break;
        }

        var roundList = stage.RoundDataList;
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

        var ingameUI = PrefabManager.Instance.UI_InGame;

        //최초 적 세팅
        CurrentRoundIndex = 0;
        currentEnemy = enemyList[CurrentRoundIndex];
        LastRoundIndex = enemyList.Count - 1;
        currentEnemy.Show();
        ingameUI.SetupProgressSlider();

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

                if (CurrentRoundIndex == LastRoundIndex)
                {
                    //마지막 놈이 죽은 경우 Loop 빠져나가자
                    CurrentGameEndType = CommonDefine.GameEndType.GameClear;
                    break;
                }

                yield return new WaitForSeconds(0.5f); //0.5초후에 스킬 선택창 보여주자

                if (CurrentRoundIndex % 3 == 1) //3번에 1번씩 보여주자....
                {
                    var selectUI = PrefabManager.Instance.UI_SelectAbility;
                    selectUI.Setup();
                    UIManager.Instance.ShowUI(selectUI);
                }

                ++CurrentRoundIndex;
                ingameUI.SetupProgressSlider();

                yield return new WaitForSeconds(1f); //1초후에 생성해주자
                //다음꺼 보여주자
                currentEnemy = enemyList[CurrentRoundIndex];
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

        SoundManager.Instance.StopAllBgmPlaying(SoundManager.StopSoundType.FadeOut);

        switch (CurrentGameEndType)
        {
            case CommonDefine.GameEndType.GameOver:
                {
                    if (GameScore > 0)
                    {
                        DataManager.Instance.Coin += GameScore / 20;
                        PrefabManager.Instance.UI_OutGame.RegisterTween = true;
                    }
                }
                break;

            case CommonDefine.GameEndType.GameClear:
                {
                    if (GameScore > 0)
                    {
                        DataManager.Instance.Coin += GameScore / 15;
                        PrefabManager.Instance.UI_OutGame.RegisterTween = true;
                    }

                    //다음 스테이지로...!
                    var currStageID = DataManager.Instance.GetSavedStageID();
                    var nextStageID = ++currStageID;
                    DataManager.Instance.SaveStageID(nextStageID);

                    if (currStageID == DataManager.Instance.GetLastStageID())
                    {
                        DataManager.Instance.IsGameClear = 1;
                        PrefabManager.Instance.UI_OutGame.RegisterGameClear = true;
                    }
                    else
                    {
                        PrefabManager.Instance.UI_OutGame.NewStageUnlocked = true;
                    }
                }
                break;
        }
    }

    public void AddScore(int score)
    {
        if (GameScore + score < int.MaxValue)
        {
            GameScore += score;

            if (player != null)
            {
                GameScore += player.CurrentCombo;
            }

            PrefabManager.Instance.UI_InGame.UpdateScore(GameScore.ToString());
        }
    }

    public Enemy_Child CurrentEnemyChild()
    {
        if (currentEnemy != null)
        {
            for (int i = currentEnemy.EnemyChildList.Count - 1; i >= 0; --i)
            {
                if (currentEnemy.EnemyChildList[i].isDeactivated == false)
                    return currentEnemy.EnemyChildList[i];
            }
        }

        return null;
    }
}
