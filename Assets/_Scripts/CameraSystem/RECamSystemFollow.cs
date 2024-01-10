using System;
using Cinemachine;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RECamSystemFollow : MonoBehaviour
    {
        public CinemachineVirtualCamera m_Camera; 

        public void Awake()
        {
            print("RECAMFOLLOWAWAKE");
            MessageBuffer<CharacterSwapCalled>.Subscribe(SetNewFollowCharacter);
        }

        private void SetNewFollowCharacter(CharacterSwapCalled msg)
        {
            print("SetFollowCharacter_" + msg.character.transform);
            m_Camera.LookAt = msg.character.transform;
            m_Camera.Follow = msg.character.transform;
        }
    }
}
