using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CharacterDoor : Door , IInteractable
    {
        [SerializeField] Character requiredCharacter;
        [SerializeField] CharacterManager characterManager;
        [SerializeField] [TextArea(3, 5)]string incorrectCharacter = "";

        public new void Interact()
        {
            if(characterManager.characterName.characterName == requiredCharacter.characterName)
            {
                OpenDoor();
            }
            else
            {
                TextDisplay(incorrectCharacter);
            }
        }
    }
}
