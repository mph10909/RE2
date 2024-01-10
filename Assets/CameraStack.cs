using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CameraStack
    {
        private static CameraStack m_Instance;

        public static CameraStack Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new CameraStack();
                return m_Instance;
            }
        }

        private RECamSystem ActiveCamera
        {
            get
            {
                return m_CameraStack.Count > 0 ? m_CameraStack[m_CameraStack.Count - 1] : null;
            }
        }

        private List<RECamSystem> m_CameraStack = new List<RECamSystem>();

        // --------------------------------------------------------------------

        public List<RECamSystem> GetCameras()
        {
            return m_CameraStack;
        }

        // --------------------------------------------------------------------

        public void AddCamera(RECamSystem cam, int priority = 0)
        {
            RECamSystem prevCam = ActiveCamera;
            if (!m_CameraStack.Contains(cam))
            {
                if (!ActiveCamera || ActiveCamera.Priority <= priority)
                {
                    m_CameraStack.Add(cam);
                }
                else
                {
                    // Insert into the list based on priority
                    int i;
                    for (i = m_CameraStack.Count - 1; i > 0; --i)
                    {
                        if (m_CameraStack[i].Priority <= priority)
                        {
                            m_CameraStack.Insert(i, cam);
                            break;
                        }
                    }
                    if (i == 0)
                        m_CameraStack.Insert(0, cam);
                }
            }

            if (ActiveCamera != prevCam)
            {
                ActiveCamera.Activate();
                if (prevCam)
                    prevCam.Deactivate();
            }
        }

        // --------------------------------------------------------------------

        public void RemoveCamera(RECamSystem cam)
        {
            RECamSystem prevCam = ActiveCamera;

            m_CameraStack.Remove(cam);
            cam.Deactivate();

            if (ActiveCamera != prevCam && ActiveCamera != null)
            {
                ActiveCamera.Activate();
            }
        }

        // --------------------------------------------------------------------

        public void ClearAllCameras()
        {
            m_CameraStack.Clear();
        }
    }
}
