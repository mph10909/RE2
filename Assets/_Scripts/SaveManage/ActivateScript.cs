using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ActivateScript : MonoBehaviour, IInteractable, IILoadable
    {
        [SerializeField] MonoBehaviour script;
        [SerializeField] bool scriptActivated;
        
        public void Interact()
        {
            print("Activate Script");
            script.enabled = true;
            scriptActivated = true;
        }

        public void Load()
        {
            if (scriptActivated) Interact();
        }
    }
}
