using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class DogRayCast : MonoBehaviour
    {
        [SerializeField] GameObject enemy;
        [SerializeField] RandomWalk randomWalk;
        [SerializeField] [Range(0,50)]float triggerDistance;
        [SerializeField] [Range(0,50)]float turnDistance;
        [SerializeField] Vector3 Left;
        [SerializeField] Vector3 Right;
        [SerializeField]
        bool lRay, rRay, straight;

        void FixedUpdate()
        {

            CastStraight();
            CastLeft();
            CastRight();
        }

        void CastStraight()
        {
            if (rRay || lRay) return;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
            {
                straight = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
                if (hit.distance < triggerDistance)
                {
                    randomWalk.CollisionDestination(hit);
                }
            }
            else straight = false;
        }

        void CastLeft()
        {
            if (rRay || straight) return;
            RaycastHit l_hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Left), out l_hit, 10))
            {
                lRay = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Left) * l_hit.distance, Color.yellow);
                if (l_hit.distance < turnDistance)
                {
                    print("LEFT");
                    //enemy.transform.Rotate(0, -1, 0);
                    randomWalk.CollisionDestination(l_hit);

                }
            }
            else lRay = false;
        }

        void CastRight()
        {
            if (lRay || straight) return;
            RaycastHit r_hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Right), out r_hit, 10))
            {
                rRay = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Right) * r_hit.distance, Color.yellow);
                if (r_hit.distance < turnDistance)
                {
                    print("RIGHT");
                    //enemy.transform.Rotate(0, 1, 0);
                    randomWalk.CollisionDestination(r_hit);
                }
            }
            else rRay = false;
        }
    }
}
