using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(menuName = "AI/States/IdleState")]
    public class IdleStateSO : EnemyStateSO
    {
        public EnemyStateSO chaseState;

        public override void OnStateEnter(EnemyAI enemy)
        {
            enemy.agent.isStopped = true;
        }

        public override void Tick(EnemyAI enemy)
        {
            if (enemy.detectedCharacters.Count > 0)
            {
                enemy.TransitionToState(chaseState);
            }
        }

        public override void OnStateExit(EnemyAI enemy)
        {
            // Any cleanup or additional logic
        }
    }


}
