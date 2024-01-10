using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntChange : StateMachineBehaviour
{
    const string ANIMATIONSELECTION = "WalkingSelection";
    const string CRAWLING = "Crawling";

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (animator.GetFloat(CRAWLING) > 0) return;
        if (animator.GetBool("LimbMissing")) return;
        int randomAnim = Random.Range(0, 4);
        animator.SetFloat(ANIMATIONSELECTION, randomAnim); 
    }

}
