using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CrackedGlass : MonoBehaviour,IDestroyable
    {

        [SerializeField] GameObject bulletHoles;

        public void Destroy()
        {
            print("Crack The Camera");
            bulletHoles.SetActive(true);
            Invoke("TurnOffHoles", 2.0f);
        }

        void TurnOffHoles()
        {
            bulletHoles.SetActive(false);
        }

    }
}
