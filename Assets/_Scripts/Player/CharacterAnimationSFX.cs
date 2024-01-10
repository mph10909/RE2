using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CharacterAnimationSFX : MonoBehaviour
    {
        AudioSource audioSource;

        [SerializeField] AudioClip zombieBite;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponentInParent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
