using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Callback : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterAction = null;
    public Action<Collider> OnTriggerExitAction = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor)
            || other.transform.CompareTag(CommonDefine.TAG_Player)
            || other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            OnTriggerEnterAction?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor)
            || other.transform.CompareTag(CommonDefine.TAG_Player)
            || other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            OnTriggerExitAction?.Invoke(other);
        }
    }


}
