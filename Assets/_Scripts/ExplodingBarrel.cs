using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ExplodingBarrel : MonoBehaviour, IDestroyable
    {
        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
