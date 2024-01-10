using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class TransformDisabler : MonoBehaviour
    {
           [SerializeField] GameObject thisTransform;

        void OnDisable()
        {
            thisTransform.SetActive(false);
        }
    }
}
