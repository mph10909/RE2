using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class TrackedDollyPointChanger : MonoBehaviour
{
    public CinemachineBrain brain;
    public List<CinemachineVirtualCamera> cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        // Set all cameras to priority 0 except the first one
        for (int i = 0; i < cameras.Count; i++)
        {
            if (i == 0)
                cameras[i].Priority = 1;
            else
                cameras[i].Priority = 0;
        }
    }

    void Update()
    {
        // Cycle through cameras using 'a' and 'd' keys
        if (Input.GetKeyDown(KeyCode.A))
        {
            cameras[currentCameraIndex].Priority = 0;
            currentCameraIndex--;
            if (currentCameraIndex < 0)
                currentCameraIndex = cameras.Count - 1;
            cameras[currentCameraIndex].Priority = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            cameras[currentCameraIndex].Priority = 0;
            currentCameraIndex++;
            if (currentCameraIndex >= cameras.Count)
                currentCameraIndex = 0;
            cameras[currentCameraIndex].Priority = 1;
        }
    }
}
