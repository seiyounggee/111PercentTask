using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using DG.Tweening;

public partial class InGameManager
{
    [SerializeField] Camera mainCam;
    [SerializeField] CinemachineVirtualCamera virtualCam;

    private void SetCamera()
    {
        mainCam = FindObjectOfType<Camera>();
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();

        if (player != null)
        {
            virtualCam.Follow = player.transform;
            virtualCam.LookAt = player.transform;
        }
    }

    public void ShakeCamera(float strength, float timer)
    {
        var perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = strength;

        UtilityInvoker.Invoke(this, () =>
        {
            var perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
        }, timer);
    }
}
