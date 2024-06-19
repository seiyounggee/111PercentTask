using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    [SerializeField] public List<Enemy_Child> EnemyChildList = null;

    public float speed = 10f;

    private Vector3 jumpUpVelocity = Vector3.zero;
    private Vector3 fallDownVelocity = Vector3.zero;

    private void Awake()
    {
        EnemyChildList = GetComponentsInChildren<Enemy_Child>().ToList();
        //뒤쪽 index가 맨 밑에 Child에 해당!
        EnemyChildList.Sort((a, b) => b.transform.localPosition.y.CompareTo(a.transform.localPosition.y));

        for (int i =0; i< EnemyChildList.Count; i++)
        {
            if (EnemyChildList[i] == null)
                continue;

            EnemyChildList[i].SetIndexNumber(i, EnemyChildList.Count - 1);
            EnemyChildList[i].PlayerCollision += PlayerCollision;
            EnemyChildList[i].PlayerDefense += PlayerDefense;
        }
    }

    private void FixedUpdate()
    {
        if (IsDead() == false)
        {
            if (jumpUpVelocity.y > 0)
            {
                this.transform.position += Vector3.up * Time.fixedDeltaTime * jumpUpVelocity.y;

                jumpUpVelocity -= Vector3.up * Time.fixedDeltaTime * speed;
                if (jumpUpVelocity.y <= 0)
                {
                    jumpUpVelocity.y = 0;
                }
            }

            if (IsGrounded() == false && jumpUpVelocity.y <= 0)
            {
                this.transform.position += Vector3.up * Time.fixedDeltaTime * fallDownVelocity.y;
                fallDownVelocity -= Vector3.up * Time.fixedDeltaTime * speed;
            }
        }
    }

    private void PlayerCollision()
    {
        jumpUpVelocity = Vector3.up * 3f;
        fallDownVelocity = Vector3.zero;
    }

    private void PlayerDefense()
    {
        jumpUpVelocity = Vector3.up * 10f;
        fallDownVelocity = Vector3.zero;
    }

    private bool IsGrounded()
    {
        foreach (var i in EnemyChildList)
        {
            if (i == null)
                continue;

            if (i.isGrounded)
                return true;
        }

        return false;
    }

    private bool IsDead()
    {
        foreach (var i in EnemyChildList)
        {
            if (i == null)
                continue;

            if (i.isDeactivated == false)
                return false;
        }

        return true;
    }
}
