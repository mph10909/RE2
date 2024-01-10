using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResidentEvilClone
{
    public class InteractableLockedObject : MonoBehaviour, IComponentSavable
    {

        [SerializeField]  private Item.ItemType item;
        [SerializeField]  Text_Data useItemText;
        [SerializeField]  [TextArea(3,5)]string noItem = "";
        [SerializeField]  [TextArea(3, 5)]string finishedText;  
        public bool activated;

        public UnityEvent activateItems;
        public UnityEvent loadActivated;

        Item.ItemType GetKeyItem()
        {
            return item;
        }

        public void Interact()
        {

            if(!CharacterManager.Instance.CheckKeyItem(item) && !activated)
            {
                UIText.Instance.StartDisplayingText(noItem, false);
            }
            else if(CharacterManager.Instance.CheckKeyItem(item) && !activated)
            {
                MessageBuffer<UITextComplete>.Subscribe(activateObject);
                UIText.Instance.StartDisplayingText(useItemText, false);
                activated = true;
            }
            else if (activated)
            {
                UIText.Instance.StartDisplayingText(finishedText, false);
            }
        }

        private void activateObject(UITextComplete ev)
        {
            activateItems?.Invoke();
        }

        public string GetSavableData()
        {
            print(activated);
            return activated.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            activated = Convert.ToBoolean(savedData);

            if (activated)
            {
                loadActivated?.Invoke();
            }
        }
    }
}
