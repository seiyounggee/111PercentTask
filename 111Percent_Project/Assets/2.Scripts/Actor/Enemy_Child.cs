using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Child : MonoBehaviour
{
    [SerializeField] Trigger_Callback bodyTrigger = null;
    [SerializeField] MeshRenderer meshRenderer = null;
    
    private Enemy enemyBase = null;
    [ReadOnly] public Material meshMaterial = null;

    [ReadOnly] public bool isGrounded = false;
    [ReadOnly] public bool isDeactivated = false;

    [ReadOnly] public int currHealth = 5;
    [ReadOnly] public int maxHealth = 5;

    public Action PlayerCollision = null;
    public Action PlayerDefense = null;

    [ReadOnly] public int indexNumber = -1;
    [ReadOnly] public int lastIndexNumber = -1;

    private void Awake()
    {
        enemyBase = GetComponentInParent<Enemy>();
        meshRenderer = GetComponent<MeshRenderer>();

        bodyTrigger = GetComponent<Trigger_Callback>();
        isDeactivated = false;

        if (meshRenderer != null)
            meshMaterial = meshRenderer.material;

        Setup(); //temp....
    }

    public void Setup()
    {
        currHealth = maxHealth;
    }

    private void OnEnable()
    {
        bodyTrigger.OnTriggerEnterAction += OnTriggerEnterAction_Body;
        bodyTrigger.OnTriggerExitAction += OnTriggerExitAction_Body;
    }

    private void OnDisable()
    {
        bodyTrigger.OnTriggerEnterAction -= OnTriggerEnterAction_Body;
        bodyTrigger.OnTriggerExitAction -= OnTriggerExitAction_Body;
    }

    public void SetIndexNumber(int myIndex, int lastIndex)
    {
        indexNumber = myIndex;
        lastIndexNumber = lastIndex;
    }

    private void Deactivate()
    {
        isDeactivated = true;
        gameObject.SafeSetActive(false);
    }

    public void GetHit(int dmg =1)
    {
        if (indexNumber < lastIndexNumber)
        {
            var formerIndexNumber = indexNumber + 1;
            if (enemyBase.EnemyChildList[formerIndexNumber].isDeactivated == false)
            {
                //�ռ� Child�� Deactivate ���°� �ƴ϶�� ���� ������ ����...
                return;
            }
        }

        currHealth -= dmg;
        if (currHealth <= 0)
        {
            currHealth = 0;
            Deactivate();
        }

        if (meshMaterial != null)
        {
            float colorValue = (float)currHealth / maxHealth;
            var newColor = new Color(colorValue, colorValue, colorValue);
            meshMaterial.SetColor("_Color", newColor);
        }
    }

    public void GetBlocked()
    {
        PlayerDefense?.Invoke();
    }

    private void OnTriggerEnterAction_Body(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = true;
        }

        if (other.transform.CompareTag(CommonDefine.TAG_Player))
        {
            PlayerCollision?.Invoke();
        }
    }

    private void OnTriggerExitAction_Body(Collider other)
    {
        if (other.transform.CompareTag(CommonDefine.TAG_Floor))
        {
            isGrounded = false;
        }
    }
}