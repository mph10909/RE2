using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{
    public class CharacterPause : MonoBehaviour
    {
        Animator anim;
        NavMeshAgent agent;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        void OnEnable()
        {
            Actions.PauseCharacter += StopCharacter;
        }

        void OnDisable()
        {
            Actions.PauseCharacter -= StopCharacter;
        }

        void StopCharacter(float playSpeed)
        {
            anim.speed = playSpeed;   
        }
    }
}
