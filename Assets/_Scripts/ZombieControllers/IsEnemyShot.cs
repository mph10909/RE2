using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class IsEnemyShot : StateMachineBehaviour
    {
        [SerializeField] bool isShot, Enter, Exit;

        const string BEENSHOT = "BeenShot"; 

       
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Enter) return;
            animator.SetBool(BEENSHOT, isShot);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Exit) return;
            animator.SetBool(BEENSHOT, isShot);
        }

    }
}
