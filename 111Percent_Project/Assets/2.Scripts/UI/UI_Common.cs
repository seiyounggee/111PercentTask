using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_Common : UIBase
{
    [SerializeField] Button btnOk = null;
    [SerializeField] Button btnExit = null;
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtDesc;

    public Action action_panelYesNo = null;

    private void Awake()
    {
        btnOk.SafeSetButton(OnClickBtn);
        btnExit.SafeSetButton(OnClickBtn);
    }

    public void Setup(string title, string desc)
    {
        txtTitle.SafeSetText(title);
        txtDesc.SafeSetText(desc);
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == btnOk)
        {
            Hide();
        }
        else if (btn == btnExit)
        {
            Hide();
        }
    }
}
