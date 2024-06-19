using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Common : UIBase
{
    [SerializeField] GameObject panelYesNo = null;
    [SerializeField] Text txt_panelYesNo = null;
    [SerializeField] Button btnYes_panelYesNo = null;
    [SerializeField] Button btnNo_panelYesNo = null;

    public Action action_panelYesNo = null;

    private void Awake()
    {
        btnYes_panelYesNo.SafeSetButton(OnClickBtnYes_panelYesNo);
        btnNo_panelYesNo.SafeSetButton(OnClickBtnNo_panelYesNo);
    }

    public void ActivatePanelYesNo(Action action, string msg = "")
    {
        gameObject.SafeSetActive(true);

        action_panelYesNo = action;
        txt_panelYesNo.text = msg;
        panelYesNo.SafeSetActive(true);
    }

    public void DeactivatePanelYesNo()
    {
        action_panelYesNo = null;
        panelYesNo.SafeSetActive(false);
    }

    private void OnClickBtnYes_panelYesNo()
    {
        action_panelYesNo?.Invoke();

        DeactivatePanelYesNo();
    }

    private void OnClickBtnNo_panelYesNo()
    {
        DeactivatePanelYesNo();
    }
}
