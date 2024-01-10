using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class TrackMover : MonoBehaviour
{
public CinemachineVirtualCamera virtualCamera;
public float pathPosition;
public float pathSpeed = 0.01f;

private CinemachineTrackedDolly trackedDolly;

private void Start()
{
    trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
}

private void Update()
{
    if (virtualCamera.Priority == 1)
    {
        pathPosition += pathSpeed * Time.deltaTime;
    }

    trackedDolly.m_PathPosition = pathPosition;
}
}

