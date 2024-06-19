using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class InGameManager
{
    [Serializable]
    public class PooledObject
    {
        public string name;
        public PooledType type;
        public GameObject obj;
        public Effect effect;
        public bool isActivated { get { if (obj != null && obj.activeSelf == true) return true; else return false; } }
    }

    private GameObject poolBase = null;
    [ReadOnly] public List<PooledObject> pooledObjList = new List<PooledObject>();

    public enum PooledType
    {
        None,
        Effect_MegaExplosionYellow,
        Effect_ShadowExplosion2,
        Effect_Hit,
        Effect_KaPow,
        Effect_Crack,
        Effect_SwordHitRedCritical,
        Effect_ConfettiBlastRainbow,
    }

    private const int INITIAL_POOL_NUMBER = 10;
    private const int ADDITIONAL_POOL_NUMBER = 5;

    private void SetPoolGameObjects(bool isAdditionalPool = false)
    {
        InitializePoolList();

        if (isAdditionalPool == false) //게임 시작 최초 1번만 실행
        {
            //Inspector창에서 정리하기위해... 
            poolBase = new GameObject();
            poolBase.transform.position = Vector3.zero;
            poolBase.name = "ObjectPooledList_BASE";
        }

        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_MegaExplosionYellow, PooledType.Effect_MegaExplosionYellow);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_ShadowExplosion2, PooledType.Effect_ShadowExplosion2);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_Hit, PooledType.Effect_Hit);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_KaPow, PooledType.Effect_KaPow);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_Crack, PooledType.Effect_Crack);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_SwordHitRedCritical, PooledType.Effect_SwordHitRedCritical);
        PoolObj(isAdditionalPool, PrefabManager.Instance.Effect_ConfettiBlastRainbow, PooledType.Effect_ConfettiBlastRainbow, 1);
              
    }

    public void InitializePoolList()
    {
        if (pooledObjList != null)
            pooledObjList.Clear();

        pooledObjList = new List<PooledObject>();

        if (poolBase != null)
            Destroy(poolBase);
    }


    private void PoolObj(bool isAdditionalPool, GameObject prefab, PooledType poolType, int poolCount = 0)
    {
        if (prefab == null)
        {
            Debug.Log("Error.... cant find prefab: " + prefab);
            return;
        }

        if (poolCount == 0)
        {
            if (isAdditionalPool == false)
            {
                poolCount = INITIAL_POOL_NUMBER; //최초 Pool
            }
            else
            {
                poolCount = ADDITIONAL_POOL_NUMBER;  //이후 추가 Pool
            }
        }
        else
        {
            if (poolCount <= 0) //혹시나해서..
                poolCount = ADDITIONAL_POOL_NUMBER;
        }

        if (pooledObjList == null)
            return;

        for (int i = 0; i <= poolCount; i++)
        {
            var go = PrefabManager.InstantiateInGamePrefab(prefab, Vector3.zero);
            go.gameObject.SafeSetActive(false);
            go.gameObject.name = go.gameObject.name + "__" + (pooledObjList.Count); ;

            if (poolBase != null)
                go.gameObject.transform.SetParent(poolBase.transform);

            //SetActiveFalse 이후에 다시 poolBase로 되돌리자...!
            Effect effectScript = null;
            if (go.GetComponent<Effect>() == null)
            {
                effectScript = go.AddComponent<Effect>();
                effectScript.SetCallback(() => { if (go != null && poolBase != null) go.transform.SetParent(poolBase.transform); });
            }
            else
            {
                effectScript = go.GetComponent<Effect>();
                effectScript.SetCallback(() => { if (go != null && poolBase != null) go.transform.SetParent(poolBase.transform); });
            }

            pooledObjList.Add(new PooledObject() { name = go.name, type = poolType, obj = go, effect = effectScript });
        }
    }


    public GameObject ActivatePooledObj(PooledType type, Vector3 posi, Quaternion rotation, float scale = 1f, Transform parentTransform = null)
    {
        if (pooledObjList != null && pooledObjList.Count > 0)
        {
            int counter = 0;
            foreach (var i in pooledObjList)
            {
                if (i.type == type && i.obj != null && i.obj.activeSelf == false)
                {
                    i.obj.transform.position = posi;
                    i.obj.transform.rotation = rotation;
                    i.obj.transform.localScale = Vector3.one * scale;
                    i.obj.SafeSetActive(true);

                    if (parentTransform == null)
                        i.obj.transform.SetParent(poolBase.transform);
                    else
                        i.obj.transform.SetParent(parentTransform);

                    return i.obj;
                }

                //마지막 index
                if (counter == pooledObjList.Count - 1)
                {
                    if (i.type != type || (i.obj != null && i.obj.activeSelf == true))
                    {
                        //ExtraPool
                        switch (type)
                        {
                            case PooledType.Effect_MegaExplosionYellow:
                                PoolObj(true, PrefabManager.Instance.Effect_MegaExplosionYellow, type);
                                break;
                            case PooledType.Effect_ShadowExplosion2:
                                PoolObj(true, PrefabManager.Instance.Effect_ShadowExplosion2, type);
                                break;
                            case PooledType.Effect_Hit:
                                PoolObj(true, PrefabManager.Instance.Effect_Hit, type);
                                break;
                            case PooledType.Effect_KaPow:
                                PoolObj(true, PrefabManager.Instance.Effect_KaPow, type);
                                break;
                            case PooledType.Effect_Crack:
                                PoolObj(true, PrefabManager.Instance.Effect_Crack, type);
                                break;
                            case PooledType.Effect_SwordHitRedCritical:
                                PoolObj(true, PrefabManager.Instance.Effect_SwordHitRedCritical, type);
                                break;
                            case PooledType.Effect_ConfettiBlastRainbow:
                                PoolObj(true, PrefabManager.Instance.Effect_ConfettiBlastRainbow, type);
                                break;
                            default:
                                break;
                        }

                        break;
                    }
                }

                ++counter;
            }
        }

        return null;
    }
}
