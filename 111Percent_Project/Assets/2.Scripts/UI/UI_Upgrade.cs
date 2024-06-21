using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Upgrade : UIBase
{
    [SerializeField] Button upgradeBtn;
    [SerializeField] Button exitBtn;

    [SerializeField] TextMeshProUGUI upgradeDmgTxt;
    [SerializeField] TextMeshProUGUI upgradeCostTxt;

    public int upgradeCost = -1;

    private void Awake()
    {
        upgradeBtn.SafeSetButton(OnClickBtn);
        exitBtn.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        Setup();
    }

    public override void Hide()
    {
        base.Hide();

        PrefabManager.Instance.UI_OutGame.SetUI();
    }

    public void Setup()
    {
        var currLv = DataManager.Instance.CurrentUpgradeLevel;
        var nextLv = ++currLv;
        var list = DataManager.Instance.UpgradeDataList;
        var data = list.Find(x => x.level.Equals(nextLv));
        if (data == null)
        {
            upgradeCostTxt.SafeSetText("MAX");
            upgradeCost = -1;
        }
        else
        {
            upgradeCost = data.cost;
            upgradeCostTxt.SafeSetText(data.cost.ToString());
            upgradeDmgTxt.SafeSetText("+" + data.damageValue.ToString());
        }
    }

    private void OnClickBtn(Button btn)
    {
        if (btn == upgradeBtn)
        {
            if (upgradeCost <= 0)
            {
                PrefabManager.Instance.UI_ToastMessage.SetMessage("Max Level");
                UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
                Hide();
                return;
            }

            if (DataManager.Instance.Coin < upgradeCost)
            {
                PrefabManager.Instance.UI_ToastMessage.SetMessage("Not Enough Money");
                UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
                Hide();
                return;
            }

            DataManager.Instance.Coin -= upgradeCost;
            DataManager.Instance.CurrentUpgradeLevel += 1;

            PrefabManager.Instance.UI_ToastMessage.SetMessage("Upgrade Success!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);

            Setup();
            Hide();
        }
        else if (btn == exitBtn)
        {
            Hide();
        }
    }
}
