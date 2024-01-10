using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class DestroyOnDisable : MonoBehaviour
    {
        void OnDisable()
        {
            Destroy(this.gameObject);
        }
    }
}
