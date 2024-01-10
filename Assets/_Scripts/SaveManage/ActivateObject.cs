using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ActivateObject : MonoBehaviour, IInteractable, IILoadable
    {
        [SerializeField] GameObject objectToActivate;
        [SerializeField] bool objectActivated;

        public void Interact()
        {
            print("This have been activated");
            objectToActivate.SetActive(true);
            objectActivated = true;
        }

        public void Load()
        {
            if (objectActivated) Interact();
        }
    }
}
