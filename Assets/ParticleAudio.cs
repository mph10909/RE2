using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ParticleAudio : MonoBehaviour
    {
        public AudioClip audioClip;
        public AudioSource audioSource;

        private void OnParticleCollision(GameObject other)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
}
