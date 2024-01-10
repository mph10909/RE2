using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class EnabledDisable : MonoBehaviour
    {
        [SerializeField]
        GameObject enableObject;


        void OnEnable()
        {
            enableObject.SetActive(false);
            print("Hi");
        }

        void OnDisable()
        {
            enableObject.SetActive(true);
            print("GoodBye");
        }
    }
}
