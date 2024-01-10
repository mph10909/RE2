using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ZombieBiteEvent : MonoBehaviour
    {
        [SerializeField]GameObject biteBlood;
        [SerializeField]Transform biteLocation;

        public void BiteBlood()
        {
            Instantiate(biteBlood, biteLocation.position, Quaternion.identity);
        }
    }
}
