using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResidentEvilClone
{
    public class TestScript : MonoBehaviour
    {
        public UnityEvent SetParent;

        void Start()
        {
            SetParent?.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
