using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class WeaponReloadBool : StateMachineBehaviour
    {
        const string FIRE = "Firing";
        const string FIRING = "Fire";
        const string RELOAD = "Reload";

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(RELOAD, true);
            animator.SetBool(FIRE, false);
            animator.ResetTrigger(FIRING);
            
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(FIRE, false);
            animator.ResetTrigger(FIRING);
            animator.SetBool(RELOAD, false);
        }

    }
}
