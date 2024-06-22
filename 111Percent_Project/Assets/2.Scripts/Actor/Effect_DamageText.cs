using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Effect_DamageText : Effect
{
    [SerializeField] TextMeshPro textMesh;

    public Vector3 initialPosition; // 초기 위치
    public Vector3 initialVelocity; // 초기 속도
    public float gravity = -9.8f; // 중력 값

    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private float time;


    public void Setup(Vector3 pos, Vector3 vel, string msg)
    {
        textMesh.color = Color.white;

        // 초기 위치와 속도를 설정
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
        // 시간 업데이트
        time += Time.fixedDeltaTime;

        // x, z 위치는 등속 운동
        currentPosition.x = initialPosition.x + initialVelocity.x * time;
        currentPosition.z = initialPosition.z + initialVelocity.z * time;

        // y 위치는 가속도 운동 (중력의 영향을 받음)
        currentPosition.y = initialPosition.y + initialVelocity.y * time + 0.5f * gravity * time * time;

        // 오브젝트 위치 업데이트
        transform.position = currentPosition;
    }

}
