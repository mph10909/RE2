using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResidentEvilClone
{
    public class LockedObject : DisplayText, IInteractable
    {

        [SerializeField]  private Item.ItemType item;
        [SerializeField]  ItemData itemData;
        [HideInInspector] public GameObject character;
        [SerializeField]  TextData useItemText;
        [SerializeField]  [TextArea(3,5)]string noItem = "";
        [SerializeField]  [TextArea(3, 5)]string finishedText;  
        [SerializeField]  GameObject[] interactable;
        [HideInInspector] public bool activated;

        [SerializeField] UnityEvent m_InteractableTrigger;

        int startingText = 0;
        bool finished,  hasItem, midText;

        Item.ItemType GetKeyItem()
        {
            return item;
        }

        public bool Activated
        {
            get { return activated; }
        }

        public override void Awake()
        {
            base.Awake();
            Actions.CharacterSwap += SetCharacter;           
        }

        void OnDisable()
        {
            Actions.CharacterSwap -= SetCharacter;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LastLine();
                NextTextLine();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if(!midText)Finished();
            }
        }

        public void Interact()
        {
            if (startingText != 0) return;
            this.enabled = true;
            finished = false;
            Time.timeScale = 0;

            if(GetKeyItem() == Item.ItemType.None)
            {
                hasItem = true;
            }
            else
            {
            foreach (Item item in character.GetComponent<PlayerInventory>().inventory.GetItemList())
            {
                if (item.itemType == GetKeyItem())
                {
                    hasItem = true;
                    break;
                }
            }
            }

            if (hasItem && !activated) { NextTextLine(); }
            if (!activated && !hasItem) { TextDisplay(noItem); }
            if (activated) { TextDisplay(finishedText); }
        }

        void NextTextLine()
        {

            if (finished || !hasItem || activated ) return;
            midText = true;
            UnclearableTextDisplay(useItemText.body[startingText]);
            startingText++;
        }

        void LastLine()
        {
            if (startingText == useItemText.body.Length - 1)
            {
                //foreach(GameObject interactables in interactable)
                //{
                //print("Interactable " + interactables.name);
                //var interacting = interactables.GetComponent<IInteractable>();
                //if (interacting != null) interacting.Interact();
                //}
                m_InteractableTrigger?.Invoke();
                activated = true;
                midText = false;
                Finished();
                return;
            }
        }

        void SetCharacter(GameObject newCharacter)
        {
            character = newCharacter;
        }

        void Finished()
        {
            print("Finished");
            finished = true;
            Time.timeScale = 1;
            TextClear();
            startingText = 0;            
            this.enabled = false;
            return;
        }


    }
}
