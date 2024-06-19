using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class InGameManager : MonoSingleton<InGameManager>
{
    [ReadOnly] public Player player = null;

    public void StartInGame()
    {
        UtilityCoroutine.StartCoroutine(ref gameRoutine, GameRoutine(), this);
    }

    private IEnumerator gameRoutine = null;
    private IEnumerator GameRoutine()
    {
        ClearStuffs();

        SetupInGameUI();

        SetPlayer();
        SetCamera();
        //SetInput();
        //SetPoolGameObjects();

        yield break;
    }

    private void ClearStuffs()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }
    }

    private void SetupInGameUI()
    {
        UIManager.Instance.HideGrouped_Outgame();

        var inGameUI = PrefabManager.Instance.UI_InGame;
        inGameUI.gameObject.SafeSetActive(true);
    }

    private void SetPlayer()
    {
        var go = GameObject.Instantiate(PrefabManager.Instance.PlayerPrefab, Vector3.zero, Quaternion.identity);
        var _player = go.GetComponent<Player>();
        if (_player != null)
        {
            player = _player;
            player.Setup();
        }
    }
}
