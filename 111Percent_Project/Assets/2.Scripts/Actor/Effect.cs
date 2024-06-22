using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum EffectType
    { 
        None,
        AutoActiveFalse,
        ManualActiveFalse,
    }
    public EffectType effectType = Effect.EffectType.None;

    [Tooltip("-1 : Infinite")]
    [SerializeField] float activeFalseTime = 3f;

    public Action callback = null;

    public virtual void OnEnable()
    {
        if (effectType == EffectType.AutoActiveFalse)
        {
            if (activeFalseTime > 0f)
                UtilityInvoker.Invoke(this, () => { Disable(); }, activeFalseTime);
        }
    }

    public virtual void OnDisable()
    { 
    
    }

    private void Disable()
    {
        transform.SafeSetActive(false);
        callback?.Invoke();
    }

    public void SetCallback(Action _callback)
    {
        callback = _callback;
    }

    public void SetTimer(float time)
    {
        activeFalseTime = time;
    }

    public void DeactivateImmediately()
    {
        UtilityInvoker.CancelInvoke(this);

        this.transform.SafeSetActive(false);
    }
}
