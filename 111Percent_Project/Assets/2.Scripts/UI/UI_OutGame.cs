using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_OutGame : UIBase
{
    [SerializeField] Button playBtn;

    private void Awake()
    {
        playBtn.SafeSetButton(OnClickBtn);
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == playBtn)
        {
            PhaseManager.Instance.ChangePhase(CommonDefine.Phase.InGame);
        }
    }
}
