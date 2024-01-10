using UnityEngine;
using System.Collections.Generic;
using Cinemachine;

public class MultiCameraPathController : MonoBehaviour
{
    public List<CinemachineVirtualCamera> virtualCameras;
    public float pathPosition;
    public float pathSpeed = 0.01f;

    private CinemachineTrackedDolly trackedDolly;
    private int currentCameraIndex = 0;

    private void Start()
    {
        trackedDolly = virtualCameras[currentCameraIndex].GetCinemachineComponent<CinemachineTrackedDolly>();
        //SetCameraPriority(currentCameraIndex, 1);
    }

    private void Update()
    {
        if (trackedDolly != null)
        {
            if (virtualCameras[currentCameraIndex].State.BlendHint == 0)
            {
                pathPosition += pathSpeed * Time.deltaTime;
            }

            foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
            {
                trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
                trackedDolly.m_PathPosition = pathPosition;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //SetCameraPriority(currentCameraIndex, 0);
                //currentCameraIndex--;
                //if (currentCameraIndex < 0)
                //{
                //    currentCameraIndex = virtualCameras.Count - 1;
                //}
                //SetCameraPriority(currentCameraIndex, 1);
                pathPosition = 0;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //SetCameraPriority(currentCameraIndex, 0);
                //currentCameraIndex++;
                //if (currentCameraIndex >= virtualCameras.Count)
                //{
                //    currentCameraIndex = 0;
                //}
                //SetCameraPriority(currentCameraIndex, 1);
                pathPosition = 0;
            }
        }
    }


    private void SetCameraPriority(int index, int priority)
    {
        CinemachineVirtualCamera virtualCamera = virtualCameras[index];
        //virtualCamera.Priority = priority;
    }
}

