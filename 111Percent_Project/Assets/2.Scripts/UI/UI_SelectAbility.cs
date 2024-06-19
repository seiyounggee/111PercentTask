using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectAbility : UIBase
{
    [SerializeField] Button leftBtn;
    [SerializeField] Button centerBtn;
    [SerializeField] Button rightBtn;

    private void Awake()
    {
        leftBtn.SafeSetButton(OnClickBtn);
        centerBtn.SafeSetButton(OnClickBtn);
        rightBtn.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        Time.timeScale = 0f;
    }

    public override void Hide()
    {
        base.Hide();

        Time.timeScale = 1f;
    }

    public void Setup()
    { 
    
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == leftBtn)
        {
            Hide();
        }
        else if (btn == centerBtn)
        {
            Hide();
        }
        else if (btn == rightBtn)
        {
            Hide();
        }
    }
}
