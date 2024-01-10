using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CharacterSpawnPoint : MonoBehaviour
    {
        public CharacterData characterToSpawn;
        public GameObject instantiatedObject;
        public bool addAtStart;

        public void RegisterPlayer(Transform newParent)
        {
            instantiatedObject = Instantiate(characterToSpawn.characterPrefab, transform.position, transform.rotation, newParent);
            if (addAtStart) CharacterManagement.Instance.AddCharacter(instantiatedObject);
        }

        public void RegisterPlayerToCharacterList()
        {
            CharacterManagement.Instance.AddCharacter(instantiatedObject);
        }
    }
}
