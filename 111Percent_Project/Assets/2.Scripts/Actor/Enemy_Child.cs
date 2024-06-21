using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Child : MonoBehaviour
{
    [SerializeField] Trigger_Callback bodyTrigger = null;
    [SerializeField] Renderer meshRenderer = null;
    
    private Enemy enemyBase = null;
    [ReadOnly] public Material meshMaterial = null;

    [ReadOnly] public bool isGrounded = false;
    [ReadOnly] public bool isDeactivated = false;
    private bool isWaitingForDeactivation = false; //연출용..

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
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

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

        isWaitingForDeactivation = false;
    }

    public void SetIndexNumber(int myIndex, int lastIndex)
    {
        indexNumber = myIndex;
        lastIndexNumber = lastIndex;
    }


    private void Deactivate()
    {
        isWaitingForDeactivation = true;

        //if (IsBoss() == false)
        if (false)
        {
            //일반은 바로 죽여주자
            Invoke(nameof(InvokeDeactivation), 0f);
        }
        else
        {
            //보스는 연출 넣어주자
            Time.timeScale = 0f;
            InGameManager.Instance.ActivatePooledObj(InGameManager.PooledType.Effect_MegaExplosionYellow, transform.position, Quaternion.identity);
            Invoke(nameof(InvokeDeactivation), 4f);
        }
    }

    private void InvokeDeactivation()
    {
        Debug.Log("InvokeDeactivation!");

        isDeactivated = true;
        isWaitingForDeactivation = false;
        Time.timeScale = 1f;
        gameObject.SafeSetActive(false);

        SoundManager.Instance.PlaySound(SoundManager.SoundClip.Ingame_EnemyDeactivation);
    }

    public void GetHit(int dmg = 10)
    {
        if (isWaitingForDeactivation == true)
            return;

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

    public bool IsBoss()
    {
        if (indexNumber != -1 && indexNumber == 0) //맨마지막..
            return true;
        else
            return false;
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
