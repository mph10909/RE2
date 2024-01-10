using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResidentEvilClone
{
    [System.Serializable]
    public class Character
    {
        public enum CharacterName
        {
            Leon,
            Claire,
            Ada,
            Chris,
            Jill,
            Ark,
            Sherry,
            Barry
        }

        public CharacterName characterName;

        public CharacterName CharactersName()
        {
            return characterName;
        }

        public Sprite CharacterSprite()
        {
            switch (characterName)
            {
                default:
                    case CharacterName.Leon:   return ItemAssets.Instance.leonSprite;
                    case CharacterName.Claire: return ItemAssets.Instance.claireSprite;
            }
        }

        public GameObject PlayerChararter()
        {
            switch (characterName)
            {
                default:
                case CharacterName.Leon:   return CharacterAssets.Instance.leonCharacter;
                case CharacterName.Claire: return CharacterAssets.Instance.claireCharacter;
            }
        }

        public GameObject CharacterCamera()
        {
            switch (characterName)
            {
                default:
                case CharacterName.Leon: return CharacterAssets.Instance.leonCamera;
                case CharacterName.Claire: return CharacterAssets.Instance.claireCamera;
            }
        }

        public Animator CharacterAnimator()
        {
            switch (characterName)
            {
                default:
                case CharacterName.Leon: return CharacterAssets.Instance.leonAnimator;
                case CharacterName.Claire: return CharacterAssets.Instance.claireAnimator;
            }
        }


    }
}
