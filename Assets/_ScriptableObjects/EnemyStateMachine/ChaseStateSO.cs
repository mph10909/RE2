using System.Collections;
using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(menuName = "AI/States/ChaseState")]
    public class ChaseStateSO : EnemyStateSO
    {
        public EnemyStateSO idleState;
        public float offset = 1f;
        private float chaseInterval = 5.0f; // Increased interval 
        private float chaseIntervalVariation = 1.0f;
        private float randomMoveChance = 0.05f; // Decreased chance of random movement 

        public override void OnStateEnter(EnemyAI enemy)
        {
            enemy.agent.isStopped = false;
            enemy.StartCoroutine(ChaseAtIntervals(enemy));
        }

        public override void Tick(EnemyAI enemy) { } // Empty Tick as logic is now in the Coroutine

        private IEnumerator ChaseAtIntervals(EnemyAI enemy)
        {
            while (true)
            {
                if (enemy.detectedCharacters.Count > 0)
                {
                    Character target = enemy.detectedCharacters[Random.Range(0, enemy.detectedCharacters.Count)];
                    Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));

                    if (Random.value < randomMoveChance)
                    {
                        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                        Vector3 randomPoint = enemy.transform.position + randomDirection * Random.Range(1f, 5f);
                        enemy.agent.SetDestination(randomPoint);
                    }
                    else if (!enemy.agent.pathPending || enemy.agent.remainingDistance < 0.5f) // If agent is close to its current destination
                    {
                        enemy.agent.SetDestination(target.transform.position + randomOffset);
                    }
                }
                else
                {
                    enemy.TransitionToState(idleState);
                }

                float randomInterval = chaseInterval + Random.Range(-chaseIntervalVariation, chaseIntervalVariation);
                yield return new WaitForSeconds(randomInterval);
            }
        }

        public override void OnStateExit(EnemyAI enemy)
        {
            enemy.StopCoroutine(ChaseAtIntervals(enemy));
        }
    }
}
