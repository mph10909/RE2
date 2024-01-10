using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class KnockBackController : MonoBehaviour
    {
        Animator Anim;

        void Awake()
        {
            Anim = GetComponent<Animator>();
        }

        private void DetectAndKnockBackOtherEnemies(float force)
        {
            

            float radius = 5.0f;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                ZombieDamageController otherEnemy = hit.collider.GetComponent<ZombieDamageController>();
                if (otherEnemy != null)
                {
                    otherEnemy.KnockedBackByEnemy(force);
                }
            }
        }

        public void KnockedBackByEnemy(float force)
        {
            print("Knock Back Other Enemies");
            Anim.SetFloat("Force", force);
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                Anim.SetTrigger("StumbleBack");
            }
            else
            {
                Anim.SetTrigger("FallBackward");
            }
        }
    }
}
