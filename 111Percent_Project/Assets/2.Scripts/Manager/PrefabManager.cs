using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoSingleton<PrefabManager>
{
    [ReadOnly] public Transform ui_parent = null;

    private void Start()
    {
        if (ui_parent == null)
        {
            ui_parent = UICanvas_BASE.Instance.transform;
        }
    }

    #region Functions

    public static GameObject InstantiateInGamePrefab(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("obj is null");
            return null;
        }

        GameObject go = Instantiate(obj);
        return go;
    }

    public static GameObject InstantiateInGamePrefab(GameObject obj, Vector3 pos, Transform parent = null)
    {
        if (obj == null)
        {
            Debug.LogError("obj is null");
            return null;
        }

        GameObject go = Instantiate(obj, parent);
        go.transform.position = pos;
        return go;
    }


    public static GameObject InstantiateInGamePrefab(GameObject obj, Vector3 pos, Vector3 rot, Transform parent = null)
    {
        if (obj == null)
        {
            Debug.LogError("obj is null");
            return null;
        }

        GameObject go = Instantiate(obj, parent);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(rot);
        return go;
    }


    public static GameObject InstantiateInGamePrefab(string path, Transform parent, Vector3 pos)
    {
        UnityEngine.Object obj = UnityEngine.Resources.Load(path);
        if (obj == null)
        {
            Debug.LogError("load failed : " + path);
            return null;
        }

        GameObject go = (GameObject)UnityEngine.Object.Instantiate(obj, parent);
        go.transform.parent = parent;
        go.transform.localPosition = pos;
        return go;
    }

    public static GameObject InstantiateUIPrefab(string path, Transform parent, Vector3 pos)
    {
        UnityEngine.Object obj = UnityEngine.Resources.Load(path);
        if (obj == null)
        {
            Debug.LogError("load failed : " + path);
            return null;
        }

        GameObject go = (GameObject)UnityEngine.Object.Instantiate(obj, parent);
        go.transform.SetParent(parent);
        go.transform.localPosition = pos;
        go.transform.localScale = Vector3.one;
        go.SetActive(false);
        return go;
    }

    #endregion //Function

    #region Prefabs

    public GameObject PlayerPrefab = null;
    public List<GameObject> EnemyPrefabList = new List<GameObject>();

    [SerializeField] public List<GameObject> PlayerSkinPrefabList = new List<GameObject>();

    [SerializeField] public List<GameObject> WeaponSkinPrefabList = new List<GameObject>();

    [SerializeField] public List<GameObject> MapPrefabList = new List<GameObject>();

    #region Effect

    public GameObject Effect_MegaExplosionYellow = null;
    public GameObject Effect_ShadowExplosion2 = null;
    public GameObject Effect_Hit = null;
    public GameObject Effect_KaPow = null;
    public GameObject Effect_Crack = null;
    public GameObject Effect_SwordHitRedCritical = null;
    public GameObject Effect_ConfettiBlastRainbow = null;
    public GameObject Effect_ExplosionNovaFire = null;
    public GameObject Effect_GibletExplodeStone = null;
    public GameObject Effect_SwordSlashThickRed = null;
    public GameObject Effect_LaserBeamRed = null;

    #endregion

    #region UI Resources

    [SerializeField] public Texture coinTexture = null;
    [SerializeField] public Texture gemTexture = null;
    #endregion

    #endregion

    #region UI Prefabs

    public UI_Title UI_Title
    {
        get
        {
            if (ui_title == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_Title", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_title = pObj.GetComponent<UI_Title>();

                    if (ui_title == null)
                        Debug.LogError("No UI_Title Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_Title");
            }

            return ui_title;
        }
    }

    private UI_Title ui_title = null;

    public UI_Logo UI_Logo
    {
        get
        {
            if (ui_logo == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_Logo", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_logo = pObj.GetComponent<UI_Logo>();
                    if (ui_logo == null)
                        Debug.LogError("No UI_Logo Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_Logo");
            }

            return ui_logo;
        }
    }

    private UI_Logo ui_logo = null;


    public UI_OutGame UI_OutGame
    {
        get
        {
            if (ui_OutGame == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_OutGame", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_OutGame = pObj.GetComponent<UI_OutGame>();
                    if (ui_OutGame == null)
                        Debug.LogError("No UI_OutGame Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_OutGame");
            }

            return ui_OutGame;
        }
    }

    private UI_OutGame ui_OutGame = null;

    public UI_Upgrade UI_Upgrade
    {
        get
        {
            if (ui_Upgrade == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_Upgrade", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_Upgrade = pObj.GetComponent<UI_Upgrade>();
                    if (ui_Upgrade == null)
                        Debug.LogError("No UI_Upgrade Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_Upgrade");
            }

            return ui_Upgrade;
        }
    }

    private UI_Upgrade ui_Upgrade = null;

    public UI_Inventory UI_Inventory
    {
        get
        {
            if (ui_Inventory == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_Inventory", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_Inventory = pObj.GetComponent<UI_Inventory>();
                    if (ui_Inventory == null)
                        Debug.LogError("No UI_Inventory Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_Inventory");
            }

            return ui_Inventory;
        }
    }

    private UI_Inventory ui_Inventory = null;

    public UI_InGame UI_InGame
    {
        get
        {
            if (ui_InGame == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_InGame", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_InGame = pObj.GetComponent<UI_InGame>();
                    if (ui_InGame == null)
                        Debug.LogError("No UI_InGame Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_InGame");
            }

            return ui_InGame;
        }
    }

    private UI_InGame ui_InGame = null;

    public UI_SelectAbility UI_SelectAbility
    {
        get
        {
            if (ui_SelectAbility == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_SelectAbility", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_SelectAbility = pObj.GetComponent<UI_SelectAbility>();
                    if (ui_SelectAbility == null)
                        Debug.LogError("No ui_SelectAbility Script Attached!");
                }
                else
                    Debug.LogError("Not Found ui_SelectAbility");
            }

            return ui_SelectAbility;
        }
    }

    private UI_SelectAbility ui_SelectAbility = null;

    public UI_Common UI_Common
    {
        get
        {
            if (ui_Common == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_Common", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_Common = pObj.GetComponent<UI_Common>();
                    if (ui_Common == null)
                        Debug.LogError("No UI_Common Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_Common");
            }

            return ui_Common;
        }
    }

    private UI_Common ui_Common = null;

    public UI_FadePanel UI_FadePanel
    {
        get
        {
            if (ui_FadePanel == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_FadePanel", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_FadePanel = pObj.GetComponent<UI_FadePanel>();
                    if (ui_FadePanel == null)
                        Debug.LogError("No UI_FadePanel Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_FadePanel");
            }

            return ui_FadePanel;
        }
    }

    private UI_FadePanel ui_FadePanel = null;

    public UI_TweenContainer UI_TweenContainer
    {
        get
        {
            if (ui_TweenContainer == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_TweenContainer", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_TweenContainer = pObj.GetComponent<UI_TweenContainer>();
                    if (ui_TweenContainer == null)
                        Debug.LogError("No UI_TweenContainer Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_TweenContainer");
            }

            return ui_TweenContainer;
        }
    }

    private UI_TweenContainer ui_TweenContainer = null;

    public UI_ToastMessage UI_ToastMessage
    {
        get
        {
            if (ui_ToastMessage == null)
            {
                GameObject pObj = InstantiateUIPrefab("Prefabs/UI/UI_ToastMessage", ui_parent, Vector3.zero);
                if (pObj != null)
                {
                    ui_ToastMessage = pObj.GetComponent<UI_ToastMessage>();
                    if (ui_ToastMessage == null)
                        Debug.LogError("No UI_ToastMessage Script Attached!");
                }
                else
                    Debug.LogError("Not Found UI_ToastMessage");
            }

            return ui_ToastMessage;
        }
    }

    private UI_ToastMessage ui_ToastMessage = null;

    #endregion
}
