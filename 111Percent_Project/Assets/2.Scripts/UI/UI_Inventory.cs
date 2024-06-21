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

    private void OnClickBtn(Button btn)
    {
        if (btn == btn_1)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 0;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_2)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 1;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_3)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 2;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_4)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 3;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_5)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 4;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }
        else if (btn == btn_6)
        {
            DataManager.Instance.CurrentWeaponSkinIndex = 5;
            PrefabManager.Instance.UI_ToastMessage.SetMessage("Weapon Changed!");
            UIManager.Instance.ShowUI(UIManager.UIType.UI_ToastMessage);
        }


        RenderTextureManager.Instance.ActivateRenderTexture_Player();
        Hide();
    }
}
