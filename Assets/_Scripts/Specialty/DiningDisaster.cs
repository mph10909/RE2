using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{
    public class DiningDisaster : MonoBehaviour
    {
        [SerializeField] GameObject[] enemies;
        [SerializeField] GameObject[] windows;
        [SerializeField] AudioClip glassShatter;
        [SerializeField] bool activated;
        

        void OnEnable()
        {
            if (activated) return;
            for(int i = 0; i < enemies.Length; i++)
                {
                print(enemies[i].name);
                var anim = enemies[i].GetComponentInChildren<Animator>();
                var nav = enemies[i].GetComponentInChildren<NavMeshAgent>();
                if (anim != null)
                {
                    anim.SetBool("BeenShot", true);
                    anim.speed = Random.Range(0.7f, 1.5f);
                    nav.speed = Random.Range(1.5f, 3.5f);

                }
                }


            for (int i = 0; i < windows.Length; i++)
            {
                var glass = windows[i].GetComponent<GlassShatter>();


                if (glass != null)
                {
                    glass.EventDestroy();
                }

            }

            SoundManagement.Instance.PlaySound(glassShatter);

            activated = true;
        }

    }
}
