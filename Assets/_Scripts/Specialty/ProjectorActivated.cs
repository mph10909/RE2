using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ProjectorActivated : MonoBehaviour, IILoadable, IInteractable
    {
        [SerializeField] GameObject thisObject;
        [SerializeField] bool activated;

        public void Interact()
        {
            print("This have been activated");
            thisObject.SetActive(true);
            activated = true;
        }

        public void Load()
        {
            if (activated)
            {
                thisObject.SetActive(true);
            }
        }
    }
}
