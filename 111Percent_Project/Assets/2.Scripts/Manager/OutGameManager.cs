using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameManager : MonoSingleton<OutGameManager>
{
    public void StartOutGame()
    {
        SetupOutGameUI();
    }

    private void SetupOutGameUI()
    {
        UIManager.Instance.HideGrouped_Ingame();

        var outGameUI = PrefabManager.Instance.UI_OutGame;
        UIManager.Instance.ShowUI(outGameUI);

        RenderTextureManager.Instance.ActivateRenderTexture_Player();
    }
}
