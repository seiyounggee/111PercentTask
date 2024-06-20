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

    [SerializeField] TextMeshProUGUI currencyCoinTxt;
    [SerializeField] TextMeshProUGUI currencyGemTxt;

    private void Awake()
    {
        playBtn.SafeSetButton(OnClickBtn);
        leftBtn.SafeSetButton(OnClickBtn);
        rightBtn.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        SetUI();
    }

    private void SetUI()
    {
        var stageID = DataManager.Instance.GetSavedStageID();
        var currentStage = DataManager.Instance.GetCurrentStageData();
        if (currentStage != null)
            stageTxt.SafeSetText(string.Format("STAGE {0} : {1}", currentStage.stageNumber, currentStage.stageName));

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
                    index = list.Count -1;
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
    }
}
