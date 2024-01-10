using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace ResidentEvilClone
{
    public class RECamSystem : MonoBehaviour
    {
        public int Priority;
        public LayerMask IgnoreLayers;

        private Cinemachine.CinemachineVirtualCamera m_VirtualCam;
        public bool TestIgnoreCollider;

        private void Awake()
        {
            MessageBuffer<CharacterSwapCalled>.Subscribe(CheckForCharacter);
            m_VirtualCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
            Deactivate();
        }

        private void CheckForCharacter(CharacterSwapCalled ev)
        {
            if ((IgnoreLayers.value & (1 << ev.character.layer)) == 0)
            {
                //print("Exit ");
                CameraStack.Instance.RemoveCamera(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TestIgnoreCollider) return;
            if ((IgnoreLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                CameraStack.Instance.AddCamera(this, Priority);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((IgnoreLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                CameraStack.Instance.RemoveCamera(this);
            }
        }

        public void Activate()
        {
            m_VirtualCam.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            m_VirtualCam.gameObject.SetActive(false);
        }
    }
}
