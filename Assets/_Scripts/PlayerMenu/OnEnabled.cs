using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class OnEnabled : MonoBehaviour
    {
        [SerializeField] Animator animator;

        void OnEnable()
        {
            animator.SetTrigger("Enable");
        }
    }
}
