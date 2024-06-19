using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private List<UIBase> activatedUIList = new List<UIBase>();

    public enum UIType
    {
        None,

        //Common UI
        UI_Logo,
        UI_CommonPanel,
        UI_FadePanel,

        //Outgame UI
        UI_OutGame,

        //Ingame UI
        UI_InGame,

    }

    public void ShowUI(UIType type)
    {
        UIBase ui = GetUI(type);

        if (ui == null)
        {
            Debug.Log("<color=red>Error...!" + type + "is null</color>");
            return;
        }

        ui.Show();

        if (activatedUIList.Contains(ui) == false)
        {
            activatedUIList.Add(ui);
        }
    }

    public void ShowUI(UIBase ui)
    {
        if (ui == null)
        {
            Debug.Log("<color=red>Error...! ui is null</color>");
            return;
        }

        ui.Show();

        if (activatedUIList.Contains(ui) == false)
        {
            activatedUIList.Add(ui);
        }
    }

    public void HideUI(UIType type)
    {
        UIBase ui = GetUI(type);

        if (ui == null)
        {
            Debug.Log("<color=red>Error...!" + type + "is null</color>");
            return;
        }

        ui.Hide();

        if (activatedUIList.Contains(ui))
            activatedUIList.Remove(ui);
    }

    public void HideUI(UIBase ui)
    {
        if (ui == null)
        {
            Debug.Log("<color=red>Error...! ui is null</color>");
            return;
        }

        ui.Hide();

        if (activatedUIList.Contains(ui))
            activatedUIList.Remove(ui);
    }

    private UIBase GetUI(UIType type)
    {
        UIBase ui = null;
        switch (type)
        {
            case UIType.None:
            default:
                break;

            case UIType.UI_Logo:
                ui = PrefabManager.Instance.UI_Logo;
                break;
            case UIType.UI_OutGame:
                ui = PrefabManager.Instance.UI_OutGame;
                break;
            case UIType.UI_InGame:
                ui = PrefabManager.Instance.UI_InGame;
                break;
            case UIType.UI_CommonPanel:
                ui = PrefabManager.Instance.UI_Common;
                break;
            case UIType.UI_FadePanel:
                ui = PrefabManager.Instance.UI_FadePanel;
                break;
        }

        return ui;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActiveBackButton();
        }
    }

    #region BackButton Handler

    public List<IBackButtonHandler> registerdBackButtonHandlerList = new List<IBackButtonHandler>();

    public void RegisterBackButton(IBackButtonHandler ui)
    {
        if (registerdBackButtonHandlerList.Contains(ui) == false)
            registerdBackButtonHandlerList.Add(ui);
    }

    public void UnregisterBackButton(IBackButtonHandler ui)
    {
        if (registerdBackButtonHandlerList.Contains(ui) == true)
            registerdBackButtonHandlerList.Remove(ui);
    }

    public void ActiveBackButton()
    {
        var currPhase = PhaseManager.Instance.CurrentPhase;

        var backBtnHandler = registerdBackButtonHandlerList.FindLast(x => true);
        if (backBtnHandler != null)
        {
            if (currPhase == CommonDefine.Phase.OutGame
                || currPhase == CommonDefine.Phase.InGame)
            {
                backBtnHandler.OnBackButton();
            }
        }
        else
        {
            if (currPhase == CommonDefine.Phase.OutGame)
            {
                var commonUI = PrefabManager.Instance.UI_Common;
                //commonUI.ActivatePanelYesNo(EndApp, "END APP?", "Do you want to Completely Quit?", true);
            }
            else if (currPhase == CommonDefine.Phase.InGame)
            {

            }
        }
    }

    private void EndApp()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif

    }


    #endregion

    #region Methods
    public void HideGroup_All()
    {
        HideGrouped_Outgame();
        HideGrouped_Ingame();
    }

    public void HideGrouped_Outgame()
    {
        HideUI(UIType.UI_Logo);
        HideUI(UIType.UI_CommonPanel);

        HideUI(UIType.UI_OutGame);
    }

    public void HideGrouped_Ingame()
    {
        HideUI(UIType.UI_InGame);
    }
    #endregion
}
