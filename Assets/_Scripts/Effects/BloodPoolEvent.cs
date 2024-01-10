using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BloodPoolEvent : MonoBehaviour
    {

        [SerializeField]GameObject bloodPool;
        [SerializeField]Transform bloodLocation;
        [SerializeField]bool bloodEnabled;

        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void EnableBloodPool()
        {
            if (bloodEnabled) return;
            Instantiate(bloodPool, bloodLocation.position, Quaternion.identity);
            bloodEnabled = true;
        }

        void HeadLessBlood()
        {
            
            if (anim.GetBool("HeadMissing"))
            {
                EnableBloodPool();
                
            }
        }
    }
}
