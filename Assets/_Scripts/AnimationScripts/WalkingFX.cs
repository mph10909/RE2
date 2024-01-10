using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingFX : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Moving", true);
    }

}
