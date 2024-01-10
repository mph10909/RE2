using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerAttackedBool : StateMachineBehaviour
    {

        [SerializeField] bool isAttacked;

        const string ATTACKED = "Attack";

        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animator.SetBool(ATTACKED, true);
            isAttacked = animator.GetBool(ATTACKED);

        }

        override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.SetBool(ATTACKED, false);
            isAttacked = animator.GetBool(ATTACKED);

        }
    }
}
