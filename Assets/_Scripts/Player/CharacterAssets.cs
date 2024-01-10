using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CharacterAssets : MonoBehaviour
    {
        public static CharacterAssets Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [Header("Characters")]
        [Space(2)][Header("Leon")]
        public GameObject leonCharacter;
        public GameObject leonCamera;
        public Animator   leonAnimator;
           
        [Space(2)][Header("Leon")]
        public GameObject claireCharacter;
        public GameObject claireCamera;
        public Animator   claireAnimator;


    }
}
