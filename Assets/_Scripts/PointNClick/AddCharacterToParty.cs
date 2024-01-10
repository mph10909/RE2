using UnityEngine;
using UnityEngine.Events;
using System;

namespace ResidentEvilClone
{
    public class AddCharacterToParty : MonoBehaviour, IComponentSavable
    {
        [TextArea(3, 10)]
        public string[] textString;

        public CharacterData character;

        public UnityEvent addCharacter;

        public UnityEvent loadData;

        public void Interact()
        {
            Debug.Log("Interact called");

            if (character == null)
            {
                Debug.LogError("CharacterData is not assigned!");
                return;
            }

            string[] modifiedTextString = new string[textString.Length];
            for (int i = 0; i < textString.Length; i++)
            {
                modifiedTextString[i] = textString[i].Replace("<color=green>insertDataHere</color>", $"<color=green>{character.characterName}</color>");
                Debug.Log($"Original: {textString[i]}, Modified: {modifiedTextString[i]}");
            }

            UIText.Instance.StartDisplayingText(modifiedTextString, false);
        }

        private void AddCharacter(UITextComplete msg)
        {

            addCharacter?.Invoke();
            this.gameObject.SetActive(false);
        }

        public string GetSavableData()
        {
            return this.gameObject.activeSelf.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            bool isActivated = Convert.ToBoolean(savedData);

            if (!isActivated)
            {
                loadData?.Invoke();
            } 
        }
    }
}
