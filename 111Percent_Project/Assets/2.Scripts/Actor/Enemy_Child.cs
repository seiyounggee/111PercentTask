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
    public Action<float> PlayerDefense = null;

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


        this.gameObject.layer = UnityEngine.LayerMask.NameToLayer(CommonDefine.LayerName_Enemy);
        gameObject.tag = CommonDefine.TAG_Enemy;
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

    public void Setup(int hp)
    {
        maxHealth = hp;
        currHealth = maxHealth;
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

        SoundManager.Instance.PlaySound(SoundManager.SoundClip.Ingame_EnemyDeactivation);
    }

    public void GetHit(int dmg = 10)
    {
        if (isDeactivated == true)
            return;

        if (indexNumber < lastIndexNumber)
        {
            var formerIndexNumber = indexNumber + 1;
            if (enemyBase.EnemyChildList[formerIndexNumber].isDeactivated == false)
            {
                //앞선 Child가 Deactivate 상태가 아니라면 공격 당하지 말자...
                return;
            }
        }

        currHealth -= dmg;
        if (currHealth <= 0)
        {
            currHealth = 0;
            Deactivate();

            var pos = this.transform.position;
            pos.z = 3f;
            InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_GibletExplodeStone, pos, Quaternion.identity);

            InGameManager.Instance.AddScore(1000);
        }
        else
            InGameManager.Instance.AddScore(100);

        if (meshMaterial != null)
        {
            float colorValue = (float)currHealth / maxHealth;
            var newColor = new Color(colorValue, colorValue, colorValue);
            meshMaterial.SetColor("_BaseColor", newColor);
        }
    }

    public void GetBlocked(float strength_percentage)
    {
        PlayerDefense?.Invoke(strength_percentage);
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
