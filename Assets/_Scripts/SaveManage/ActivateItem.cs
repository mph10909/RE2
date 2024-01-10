using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ActivateItem : MonoBehaviour, IInteractable
    {
        [SerializeField] Animator objectToActivate;
        

        public void Interact()
        {
            objectToActivate.enabled = true;
        }

    }
}
