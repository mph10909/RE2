using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CheckForPlayer : MonoBehaviour
    {
        //Make sure to assign this in the Inspector window
        public Transform m_NewTransform;
        public Transform thisArea;
        Collider m_Collider;

        void Start()
        {
            m_Collider = GetComponent<Collider>();

        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;
            if (m_Collider.bounds.Contains(m_NewTransform.position))
            {
                print("I Am In " + this.name);
            }
            else print("Not In");
        }
    }
}
