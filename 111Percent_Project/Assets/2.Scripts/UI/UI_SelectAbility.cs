using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class UI_SelectAbility : UIBase
{
    [SerializeField] Button leftBtn;
    [SerializeField] TextMeshProUGUI nameTxt_left;
    [SerializeField] TextMeshProUGUI descTxt_left;

    [SerializeField] Button centerBtn;
    [SerializeField] TextMeshProUGUI nameTxt_center;
    [SerializeField] TextMeshProUGUI descTxt_center;

    [SerializeField] Button rightBtn;
    [SerializeField] TextMeshProUGUI nameTxt_right;
    [SerializeField] TextMeshProUGUI descTxt_right;

    private List<DataManager.AbilityData> selectedAbilityList = new List<DataManager.AbilityData>();

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
        var list = DataManager.Instance.AbilityDataList;
        selectedAbilityList = list.OrderBy(x => Guid.NewGuid()).Take(3).ToList(); //랜덤으로 선택

        if (selectedAbilityList != null && selectedAbilityList.Count == 3)
        {
            nameTxt_left.SafeSetText(selectedAbilityList[0].name);
            descTxt_left.SafeSetText(selectedAbilityList[0].desc);

            nameTxt_center.SafeSetText(selectedAbilityList[1].name);
            descTxt_center.SafeSetText(selectedAbilityList[1].desc);

            nameTxt_right.SafeSetText(selectedAbilityList[2].name);
            descTxt_right.SafeSetText(selectedAbilityList[2].desc);
        }

    }

    private void OnClickBtn(Button btn)
    {
        var player = InGameManager.Instance.player;

        if (btn == leftBtn)
        {
            if (player != null)
                player.GetAbility(selectedAbilityList[0]);

            Hide();

        }
        else if (btn == centerBtn)
        {
            if (player != null)
                player.GetAbility(selectedAbilityList[1]);

            Hide();
        }
        else if (btn == rightBtn)
        {
            if (player != null)
                player.GetAbility(selectedAbilityList[2]);

            Hide();

        }
    }
}
