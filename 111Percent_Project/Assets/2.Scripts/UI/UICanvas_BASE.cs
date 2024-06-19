using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_BASE : MonoSingleton<UICanvas_BASE>
{
    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }
}
