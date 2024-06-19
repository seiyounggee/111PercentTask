using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_BASE : MonoSingleton<Manager_BASE>
{
    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }
}
