using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ColliderEnabler : MonoBehaviour
    {

        [SerializeField] Collider collide;
        [SerializeField] bool enter,exit;

        void OnTriggerEnter(Collider col)
        {
            if(enter)
                collide.enabled = true;
        }

        void OnTriggerExit(Collider Col)
        {
            if (exit)
                collide.enabled = false;
        }

    }
}
