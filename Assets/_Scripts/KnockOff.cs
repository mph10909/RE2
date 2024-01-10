using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class KnockOff : StateMachineBehaviour
    {
        const string KNOCKOFF = "KnockOff";

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            int isknockedOff = Random.Range(0, 2);
            animator.SetInteger(KNOCKOFF, isknockedOff);
        }
    }


}