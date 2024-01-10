using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : StateMachineBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("LimbMissing"))
        {
            animator.SetInteger("Health", 0);
        }
    }

}
