using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ZombieSoundEvents : MonoBehaviour
    {
        ZombieBiteEvent biteBlood;
        Animator anim;
        AudioSource audioSource;
        [SerializeField] AudioClip moan, moan2, thud, bite, quickMoan, death;
        [SerializeField] AudioClip[] moans;
        [SerializeField] int walkingMoan;
        [SerializeField]float animSpeed;
        int moanCounter;

        bool HeadMissing
        {
            get { return anim.GetBool("HeadMissing"); }
        }


        void Start()
        {      
            walkingMoan = Random.Range(7, 14);
            biteBlood = GetComponent<ZombieBiteEvent>();
            anim = GetComponent<Animator>();
            audioSource = GetComponentInParent<AudioSource>();
            anim.speed = Random.Range(0.8f, 1.2f);
        }

        void ZombieMoan()
        {
            if (HeadMissing) return;
            audioSource.PlayOneShot(moan);
        }

        void Zombiethud()
        {

            audioSource.PlayOneShot(thud);
        }

        void ZombieBite()
        {
            audioSource.PlayOneShot(bite);
            biteBlood.BiteBlood();
        }

        void ZombieQuickMoan()
        {
            if (HeadMissing) return;
            audioSource.PlayOneShot(quickMoan);
        }

        void RandomMoan()
        {
            if (HeadMissing) return;
            AudioClip randomMoan = moans[Random.Range(0, moans.Length)];
            audioSource.PlayOneShot(randomMoan);
        }

        void WalkingMoan()
        {
            moanCounter++;        
            if (moanCounter == walkingMoan)
            {
                audioSource.PlayOneShot(moan);
                moanCounter = 0;
            }
        }
    }
}
