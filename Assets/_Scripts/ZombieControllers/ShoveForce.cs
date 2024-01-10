using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ShoveForce : StateMachineBehaviour
    {
        [SerializeField] int force;

        override public void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            force = Random.Range(1100, 1500);
            anim.SetFloat("Force", force);
        }

    }
}
