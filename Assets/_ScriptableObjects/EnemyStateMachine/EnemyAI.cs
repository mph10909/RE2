using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace EnemyAI
{
    public class EnemyAI : MonoBehaviour
    {
        public EnemyStateSO startState;
        public List<Character> detectedCharacters = new List<Character>();
        private EnemyStateSO currentState;

        public NavMeshAgent agent;

        private TriggerObserver triggerObserver;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            TransitionToState(startState);
        }

        private void Update()
        {
            currentState.Tick(this);
        }

        public void TransitionToState(EnemyStateSO newState)
        {
            currentState?.OnStateExit(this);
            currentState = newState;
            currentState?.OnStateEnter(this);
        }

        public void OnTriggerDetected(Collider collider)
        {
            Character character = collider.GetComponent<Character>();
            if (character && !detectedCharacters.Contains(character))
            {
                detectedCharacters.Add(character);
            }
        }

        public void OnTriggerLeft(Collider collider)
        {
            Character character = collider.GetComponent<Character>();
            if (character)
            {
                detectedCharacters.Remove(character);
            }
        }
    }

}
