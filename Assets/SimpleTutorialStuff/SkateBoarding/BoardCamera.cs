using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ResidentEvilClone
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera groundCam;
        [SerializeField] OllieTest ollieTest;

        void Update()
        {
            var Body = groundCam.GetCinemachineComponent<CinemachineTransposer>();

            if (!ollieTest.IsGrounded())
            {
               
                Body.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            }
            else
            {
                Body.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
            }
        }
    }
}
