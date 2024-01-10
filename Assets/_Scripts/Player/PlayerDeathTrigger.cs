using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerDeathTrigger : StateMachineBehaviour
    {

        [SerializeField] bool frontDeath;
        [SerializeField] bool backDeath;
        const string HEALTH = "Health";
        const string DEATH = "Death";
        const string FRONTDEATH = "FrontDeath";
        const string BACKDEATH = "BackDeath";



        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetFloat(HEALTH) > 0.0f) return;
            if (frontDeath)
            {
                animator.SetTrigger(DEATH);
                animator.SetTrigger(FRONTDEATH);
            }
            if (backDeath)
            {
                animator.SetTrigger(DEATH);
                animator.SetTrigger(BACKDEATH);
            }

        }


    }
}
