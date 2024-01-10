using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerFiring : StateMachineBehaviour
    {
        const string FIRING = "Firing";
        [SerializeField] bool firing;

        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            
            animator.SetBool(FIRING, true);
            firing = animator.GetBool(FIRING);
        }


        override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            
            animator.SetBool(FIRING, false);
            firing = animator.GetBool(FIRING);
        }
    }
}
