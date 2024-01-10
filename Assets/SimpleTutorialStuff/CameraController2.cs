using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController2 : MonoBehaviour
{
    public List<Transform> cameraPoints;
    private int currentIndex = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentIndex--;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentIndex++;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, cameraPoints.Count - 1);
    }

    void LateUpdate()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.transform.position = cameraPoints[currentIndex].position;
    }
}
