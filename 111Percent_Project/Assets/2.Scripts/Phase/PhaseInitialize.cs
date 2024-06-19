using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseInitialize : PhaseBase
{
    IEnumerator phaseInitializeCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        UtilityCoroutine.StartCoroutine(ref phaseInitializeCoroutine, PhaseInitializeCoroutine(), this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

    }

    IEnumerator PhaseInitializeCoroutine()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        yield return null;

        //Load Local Data...
        //Load Ref
        //Load DB
        //Login

        
        yield return new WaitForSeconds(1f);

        PhaseManager.Instance.ChangePhase(CommonDefine.Phase.OutGame);
    }
}
