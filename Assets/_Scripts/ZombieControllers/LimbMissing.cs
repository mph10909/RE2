using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class LimbMissing : StateMachineBehaviour
    {

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetBool("HeadMissing")) return;
            if (animator.GetBool("LimbMissing"))
            {
                animator.SetFloat("ArmOut", 0);
                animator.SetFloat("WalkingSelection", 0);
            }
        }

        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (animator.GetBool("HeadMissing")) return;
        //    if (animator.GetBool("LimbMissing"))
        //    {
        //        animator.SetFloat("Crawling", 1);
        //        animator.SetFloat("WalkingSelection", 0);
        //    }

        //}

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetBool("HeadMissing")) return;
            if (animator.GetBool("LimbMissing"))
            {
                animator.SetFloat("WalkingSelection", 0);
            }
        }


    }
}
