using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Bullet : Effect
{
    [SerializeField] Trigger_Callback bodyTrigger = null;

    private Transform targetTrans;

    private Vector3 startDir = Vector3.up;

    private void Awake()
    {
        bodyTrigger = GetComponent<Trigger_Callback>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        bodyTrigger.OnTriggerEnterAction += OnTriggerEnterAction_Body;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        bodyTrigger.OnTriggerEnterAction -= OnTriggerEnterAction_Body;
    }

    public void Setup(Transform start, Transform target)
    {
        transform.position = start.position;
        targetTrans = target;

        startDir = (Vector3.up + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f))).normalized;
    }

    private void LateUpdate()
    {
        if (targetTrans != null)
        {
            var dir = (targetTrans.transform.position - this.transform.position).normalized;
            var lerpedDir = Vector3.Lerp(startDir, dir, Time.deltaTime * 10f);
            transform.position += lerpedDir * Time.deltaTime * 25f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }
    }

    private void OnTriggerEnterAction_Body(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Enemy))
        {
            InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_Hit, transform.position, Quaternion.identity);
            DeactivateImmediately();
        }
    }
}
