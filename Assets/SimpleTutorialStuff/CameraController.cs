using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ResidentEvilClone
{

    public class CameraController : MonoBehaviour
    {
        public CinemachineBrain brain;
        public List<CinemachineVirtualCamera> camerasList1;
        public List<CinemachineVirtualCamera> camerasList2;
        private List<CinemachineVirtualCamera> currentCameras;

        private CinemachineTrackedDolly trackedDolly;
        private int currentCameraIndex = 0;
        private bool isList1Active = true;
        private bool isSecondListActive = false;

        public float pathPosition;
        public float pathSpeed = 0.01f;

        void Start()
        {
            trackedDolly = camerasList2[currentCameraIndex].GetCinemachineComponent<CinemachineTrackedDolly>();
            // Set all cameras to priority 0 except the first one
            currentCameras = isList1Active ? camerasList1 : camerasList2;
            for (int i = 0; i < currentCameras.Count; i++)
            {
                if (i == 0)
                    currentCameras[i].Priority = 1;
                else
                    currentCameras[i].Priority = 0;
            }
        }

        void Update()
        {
            if (trackedDolly != null)
            {
                if (camerasList2[currentCameraIndex].State.BlendHint == 0)
                {
                    pathPosition += pathSpeed * Time.deltaTime;
                }

                foreach (CinemachineVirtualCamera virtualCamera in camerasList2)
                {
                    trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
                    trackedDolly.m_PathPosition = pathPosition;
                }
            }
            if (isList1Active && Input.GetKeyDown(KeyCode.E))
            {
                // Switching from camera list 2 to camera list 1
                pathPosition = 0;
            }
            // Cycle through cameras using 'a' and 'd' keys
            if (!isSecondListActive && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                currentCameras[currentCameraIndex].Priority = 0;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentCameraIndex--;
                    if (currentCameraIndex < 0)
                        currentCameraIndex = currentCameras.Count - 1;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    currentCameraIndex++;
                    if (currentCameraIndex >= currentCameras.Count)
                        currentCameraIndex = 0;
                }
                currentCameras[currentCameraIndex].Priority = 1;
            }
            // Toggle between camera lists using 'e' key
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Set current camera priority to 0 and switch to other list
                currentCameras[currentCameraIndex].Priority = 0;
                isList1Active = !isList1Active;
                currentCameras = isList1Active ? camerasList1 : camerasList2;
                // Match the current camera index to the other list
                currentCameraIndex = Mathf.Clamp(currentCameraIndex, 0, currentCameras.Count - 1);
                // Set current camera to priority 1
                currentCameras[currentCameraIndex].Priority = 1;
                // Update second list active status
                isSecondListActive = currentCameras == camerasList2;


            }

            // Toggle camera priority between 0 and 1 for the current camera in the second list using 'e' key
            else if (isSecondListActive && Input.GetKeyDown(KeyCode.E))
            {
                if (currentCameras[currentCameraIndex].Priority == 0)
                {
                    // Set current camera to priority 1 and the previous camera to priority 0
                    for (int i = 0; i < currentCameras.Count; i++)
                    {
                        if (currentCameras[i].Priority == 1)
                            currentCameras[i].Priority = 0;
                    }
                    currentCameras[currentCameraIndex].Priority = 1;
                }
                else
                {
                    // Set current camera to priority 0
                    currentCameras[currentCameraIndex].Priority = 0;
                }
            }
        }
    }
}
