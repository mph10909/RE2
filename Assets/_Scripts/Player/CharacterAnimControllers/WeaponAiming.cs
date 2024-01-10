using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class WeaponAiming : StateMachineBehaviour
    {
        [SerializeField] bool isAiming;
        const string AIMING = "Aiming";

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(AIMING, isAiming);

        }

    }
}
