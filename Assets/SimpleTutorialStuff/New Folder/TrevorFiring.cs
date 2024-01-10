using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TrevorFiring : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isFiring", true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isFiring", false);
    }
}
