using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    private GameObject playerObject;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        playerObject = PlayerStats.Instance.gameObject.transform.GetChild(1).gameObject;
        virtualCamera.Follow = playerObject.transform;
        virtualCamera.LookAt = playerObject.transform;
    }
}
