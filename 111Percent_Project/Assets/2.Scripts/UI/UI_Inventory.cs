using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UIBase
{
    [SerializeField] Button exitBtn;

    [SerializeField] Button btn_1;
    [SerializeField] Button btn_2;
    [SerializeField] Button btn_3;
    [SerializeField] Button btn_4;
    [SerializeField] Button btn_5;
    [SerializeField] Button btn_6;

    [SerializeField] GameObject[] lockObj;

    private void Awake()
    {
        exitBtn.SafeSetButton(OnClickBtn);
        btn_1.SafeSetButton(OnClickBtn);
        btn_2.SafeSetButton(OnClickBtn);
        btn_3.SafeSetButton(OnClickBtn);
        btn_4.SafeSetButton(OnClickBtn);
        btn_5.SafeSetButton(OnClickBtn);
        btn_6.SafeSetButton(OnClickBtn);
    }

    public override void Show()
    {
        base.Show();

        var data = DataManager.Instance.GetCurrentStageData();
        for (int i = 0; i < lockObj.Length; i++)
        {
            lockObj[i].SafeSetActive(i + 1 >= data.stageNumber);
        }
    }

    private void OnClickBtn(Button btn)
    {
        var data = DataManager.Instance.GetCurrentStageData();


        if (btn == btn_1)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 0;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Goblin Mace Equiped!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_2)
        {
            if (data.stageNumber <= 2)
                return;

            DataManager.Instance.CurrentWeaponSkinIndex = 1;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Goblin Staff Equiped!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_3)
        {
            if (data.stageNumber <= 3)
                return;

            DataManager.Instance.CurrentWeaponSkinIndex = 2;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Axe Equiped!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_4)
        {
            if (data.stageNumber <= 4)
                return;

            DataManager.Instance.CurrentWeaponSkinIndex = 3;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Human Mace Equiped!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_5)
        {
            if (data.stageNumber <= 5)
                return;

            DataManager.Instance.CurrentWeaponSkinIndex = 4;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Undead Staff Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_6)
        {
            if (DataManager.Instance.IsGameClear == 0)
                return;

            DataManager.Instance.CurrentWeaponSkinIndex = 5;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Wood Sword Equiped!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }


        RenderTextureManager.Instance.ActivateRenderTexture_Player();
        Hide();
    }
}
