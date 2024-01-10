using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStanding : StateMachineBehaviour
{
    [SerializeField] bool isStanding, onEnter, onExit;

    const string STANDING = "Standing";


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit) return;
        animator.SetBool(STANDING, isStanding);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onEnter) return;
        animator.SetBool(STANDING, isStanding);
    }

}
