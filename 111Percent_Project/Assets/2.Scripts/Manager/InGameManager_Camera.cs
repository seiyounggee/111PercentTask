using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

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
}
