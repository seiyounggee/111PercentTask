using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_OutGame : UIBase
{
    [SerializeField] Button playBtn;
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;

    [SerializeField] TextMeshProUGUI stageTxt;

    [SerializeField] Transform currencyCoinTrans;
    [SerializeField] TextMeshProUGUI currencyCoinTxt;
    [SerializeField] Transform currencyGemTrans;
    [SerializeField] TextMeshProUGUI currencyGemTxt;

    [SerializeField] Button shopBtn;
    [SerializeField] Button inventoryBtn;
    [SerializeField] Button upgradeBtn;

    [ReadOnly] public bool RegisterTween = false;
    [ReadOnly] public bool NewStageUnlocked = false;
    [ReadOnly] public bool RegisterGameClear = false;

    private void Awake()
    {
        playBtn.SafeSetButton(OnClickBtn);
        leftBtn.SafeSetButton(OnClickBtn);
        rightBtn.SafeSetButton(OnClickBtn);

        shopBtn.SafeSetButton(OnClickBtn);
        inventoryBtn.SafeSetButton(OnClickBtn);
        upgradeBtn.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        SetUI();

        TweenCurrency();
        NewStageUnlockedMsg();
        GameClearPopup();
    }

    public void SetUI()
    {
        var stageID = DataManager.Instance.GetSavedStageID();
        var currentStage = DataManager.Instance.GetCurrentStageData();
        if (currentStage != null)
        {
            if (DataManager.Instance.IsGameClear == 0)
                stageTxt.SafeSetText(string.Format("STAGE {0} : {1}", currentStage.stageNumber, currentStage.stageName));
            else
                stageTxt.SafeSetText("ALL CLEAR!");
        }

        currencyCoinTxt.SafeSetText(DataManager.Instance.Coin.ToString());
        currencyGemTxt.SafeSetText(DataManager.Instance.Gem.ToString());
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == playBtn)
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.InGame);
        }
        else if (btn == rightBtn)
        {
            if (DataManager.Instance.GetSavedSkinPrefab() == null)
                return;

            var list = PrefabManager.Instance.PlayerSkinPrefabList;
            var index = list.FindIndex(x => x.Equals(DataManager.Instance.GetSavedSkinPrefab()));
            if (index != -1)
            {
                if (index == list.Count - 1)
                {
                    index = 0;
                    DataManager.Instance.SaveSkinPrefab(list[index].name);
                }
                else
                {
                    ++index;
                    DataManager.Instance.SaveSkinPrefab(list[index].name);
                }

                RenderTextureManager.Instance.ActivateRenderTexture_Player();
            }
        }
        else if (btn == leftBtn)
        {
            if (DataManager.Instance.GetSavedSkinPrefab() == null)
                return;

            var list = PrefabManager.Instance.PlayerSkinPrefabList;
            var index = list.FindIndex(x => x.Equals(DataManager.Instance.GetSavedSkinPrefab()));
            if (index != -1)
            {
                if (index == 0)
                {
                    index = list.Count - 1;
                    DataManager.Instance.SaveSkinPrefab(list[index].name);
                }
                else
                {
                    --index;
                    DataManager.Instance.SaveSkinPrefab(list[index].name);
                }

                RenderTextureManager.Instance.ActivateRenderTexture_Player();
            }
        }
        else if (btn == shopBtn)
        {
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Not Ready Yet...!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == inventoryBtn)
        {
            UIManager.Instance.ShowUI(UIManager.UIType.UI_Inventory);
        }
        else if (btn == upgradeBtn)
        {
            UIManager.Instance.ShowUI(UIManager.UIType.UI_Upgrade);
        }
    }

    private void TweenCurrency()
    {
        if (RegisterTween)
        {
            var uiTween = PrefabManager.Instance.UI_TweenContainer;
            var coinTexture = PrefabManager.Instance.coinTexture;

            uiTween.Tween(coinTexture, playBtn.transform, currencyCoinTrans, 0, DataManager.Instance.Coin, group_count: 5, single_count: 5);

            RegisterTween = false;
        }
    }

    private void NewStageUnlockedMsg()
    {
        if (NewStageUnlocked)
        {
            var currentStage = DataManager.Instance.GetCurrentStageData();
            if (currentStage != null)
            {
                var msg = string.Format("New Stage {0} Unlocked!", currentStage.stageName);
                PrefabManager.Instance.UI_ToastMessage.SetMessage(msg);
                UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
            }

            NewStageUnlocked = false;
        }
    }

    private void GameClearPopup()
    {
        if (RegisterGameClear)
        {
            var ui = PrefabManager.Instance.UI_Common;
            ui.Setup("Game Clear", "Thank you for playing!");
            UIManager.Instance.ShowUI(ui);

            RegisterGameClear = false;
        }
    }
}
