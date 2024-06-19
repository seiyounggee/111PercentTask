using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UI_InGame : UIBase
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI comboTxt;

    [SerializeField] List<GameObject> healthObj = new List<GameObject>();

    public override void Show()
    {
        base.Show();

        Clear();
    }

    public void Clear()
    {
        scoreTxt.SafeSetText(string.Empty);
        comboTxt.SafeSetText(string.Empty);
    }
}
