using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{
    public class RandomWalk : MonoBehaviour
    {
        [SerializeField] NavMeshAgent enemyNav;
        [SerializeField][Range(0,100)] float speed;
        [SerializeField][Range(0,300)] float walkRadius;


        void Start()
        {
            enemyNav = GetComponent<NavMeshAgent>();
            if(enemyNav != null)
            {
                enemyNav.speed = speed;
                enemyNav.SetDestination(RandomLocation());
            }
        }

        public Vector3 RandomLocation()
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitCircle * walkRadius;
            randomPosition += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, walkRadius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }


        void Update()
        {
            if(enemyNav !=null && enemyNav.remainingDistance <= enemyNav.stoppingDistance)
            {
                enemyNav.SetDestination(RandomLocation());
            }
        }

        public void CollisionDestination(RaycastHit hit)
        {
            print(hit.collider.gameObject.name);
            enemyNav.SetDestination(RandomLocation());
        }
    }
}
