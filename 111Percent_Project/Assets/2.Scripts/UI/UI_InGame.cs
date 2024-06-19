using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class UI_InGame : UIBase
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI comboTxt;

    [SerializeField] List<GameObject> healthObj = new List<GameObject>();
    
    [SerializeField] GameObject hitUI = null;

    [SerializeField] GameObject gameOverPopup = null;
    [SerializeField] Button gameOverBtn = null;

    [SerializeField] GameObject gameClearPopup = null;
    [SerializeField] Button gameClearBtn = null;

    private void Awake()
    {
        gameOverBtn.SafeSetButton(OnClickBtn);
        gameClearBtn.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        Clear();
    }

    public void Clear()
    {
        scoreTxt.SafeSetText(string.Empty);
        comboTxt.SafeSetText(string.Empty);

        foreach (var i in healthObj) 
        {
            i.SafeSetActive(true);
        }

        hitUI.SafeSetActive(false);

        gameOverPopup.SafeSetActive(false);
        gameClearPopup.SafeSetActive(false);
    }

    public void UpdateHealthObj()
    {
        var player = InGameManager.Instance.player;
        if (player != null)
        {
            for (int i = 0; i < healthObj.Count; i++)
            {
                healthObj[i].SafeSetActive(false);

                if (i <= player.currHealth - 1)
                {
                    healthObj[i].SafeSetActive(true);
                }
            }
        }
    }

    public void ActivateGameOver()
    {
        gameOverPopup.SafeSetActive(true);
    }

    public void ActivateGameClear()
    {
        gameClearPopup.SafeSetActive(true);
    }

    public void ActivateHitUI()
    {
        hitUI.GetComponent<Image>().color = new Color32(255, 0, 0, 143);
        hitUI.SafeSetActive(false);
        hitUI.SafeSetActive(true);
        
        hitUI.transform.DORestart();

        UtilityInvoker.Invoke(this, () =>
        {
            hitUI.SafeSetActive(false);
        }, 0.3f);
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == gameOverBtn)
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.OutGame);
        }
        else if (btn == gameClearBtn) 
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.OutGame);
        }
    }
}
