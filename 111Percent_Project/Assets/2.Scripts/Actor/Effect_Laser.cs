using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Laser : Effect
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject laserEnd;

    private Transform fromTrans;
    private Transform toTrans;

    public void Setup(Transform _fromTrans, Transform _toTrans)
    {
        lineRenderer.positionCount = 2;

        fromTrans = _fromTrans;
        toTrans = _toTrans;
    }


    private void LateUpdate()
    {
        if (lineRenderer != null && lineRenderer.positionCount >= 2
            && fromTrans != null && toTrans != null)
        {
            lineRenderer.SetPosition(0, fromTrans.transform.position + Vector3.up);
            lineRenderer.SetPosition(1, toTrans.transform.position);

            if (laserEnd != null)
            {
                laserEnd.transform.position = toTrans.transform.position;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
