using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [CreateAssetMenu(menuName = "ResidentEvilClone/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public Character character;
        public GameObject characterPrefab;
        public Sprite characterPortrait;
        // Add other character-specific data as needed
    }

}
