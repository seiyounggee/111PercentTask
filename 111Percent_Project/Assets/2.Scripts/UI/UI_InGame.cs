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
        scoreTxt.SafeSetText("0");
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
        UtilityInvoker.Invoke(this, () =>
        {
            gameOverPopup.SafeSetActive(true);
        }, 2f, "gameoverPopup");

    }

    public void ActivateGameClear()
    {
        UtilityInvoker.Invoke(this, () =>
        {
            gameClearPopup.SafeSetActive(true);
        }, 2f, "gameClearPopup");
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

    public void UpdateCombo(string msg)
    {
        if (string.IsNullOrEmpty(msg))
        {
            comboTxt.gameObject.SafeSetActive(false);
        }
        else
        {
            comboTxt.gameObject.SafeSetActive(true);
            comboTxt.SafeSetText(msg);

            comboTxt.transform.DOScale(Vector3.one, 0f);
            comboTxt.transform.DOPunchScale(Vector3.one * 1.05f, 0.2f);

            UtilityInvoker.Invoke(this, () =>
            {
                comboTxt.gameObject.SafeSetActive(false);
            }, 1f, "combo");
        }
    }

    public void UpdateScore(string msg)
    {
        scoreTxt.gameObject.SafeSetActive(true);
        scoreTxt.SafeSetText(msg);
        scoreTxt.transform.DOScale(Vector3.one, 0f);
        scoreTxt.transform.DOPunchScale(Vector3.one * 1.15f, 0.2f);
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
