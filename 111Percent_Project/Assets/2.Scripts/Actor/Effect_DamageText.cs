using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Effect_DamageText : Effect
{
    [SerializeField] TextMeshPro textMesh;

    public Vector3 initialPosition; // �ʱ� ��ġ
    public Vector3 initialVelocity; // �ʱ� �ӵ�
    public float gravity = -9.8f; // �߷� ��

    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private float time;


    public void Setup(Vector3 pos, Vector3 vel, string msg)
    {
        textMesh.color = Color.white;

        // �ʱ� ��ġ�� �ӵ��� ����
        initialPosition = pos;
        initialVelocity = vel;
        currentPosition = initialPosition;
        currentVelocity = initialVelocity;
        time = 0;

        gravity = -25f;

        textMesh.text = msg;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        textMesh.DOFade(0f, 2f);
    }

    void FixedUpdate()
    {
        // �ð� ������Ʈ
        time += Time.fixedDeltaTime;

        // x, z ��ġ�� ��� �
        currentPosition.x = initialPosition.x + initialVelocity.x * time;
        currentPosition.z = initialPosition.z + initialVelocity.z * time;

        // y ��ġ�� ���ӵ� � (�߷��� ������ ����)
        currentPosition.y = initialPosition.y + initialVelocity.y * time + 0.5f * gravity * time * time;

        // ������Ʈ ��ġ ������Ʈ
        transform.position = currentPosition;
    }

}
