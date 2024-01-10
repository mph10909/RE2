using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class SetInventoryButtons : MonoBehaviour
    {
        public Item item;
        public int ammo;
        [SerializeField] CharacterManager characterManager;
        [SerializeField] GameObject[] buttonItem;
        List<Button> buttons = new List<Button>();

        [HideInInspector]public PlayerInventory inventory;
        [HideInInspector]public AmmoInventory ammoInventory;

        void Awake()
        {       
            foreach(GameObject item in buttonItem)
            {
                buttons.Add(item.gameObject.GetComponent<Button>());
            }
        }

        void OnEnable()
        {
            ItemStorageManager.ButtonEnable += ButtonEnabler;
            Actions.InventoryItemChange += RefreshStorageInventory;
            SetInventory(characterManager.currentPlayer);
            RefreshStorageInventory();
            ButtonEnabler(true);
        }

        void OnDisable()
        {
            ItemStorageManager.ButtonEnable -= ButtonEnabler;
            Actions.InventoryItemChange -= RefreshStorageInventory;
            item = new Item { itemType = Item.ItemType.None, amount = 0 };
            ClearInventoryButtons();
        }

        void SetInventory(GameObject selectedPlayer)
        {
            inventory     = selectedPlayer.GetComponent<PlayerInventory>();
            ammoInventory = selectedPlayer.GetComponent<AmmoInventory>();
        }

        private void RefreshStorageInventory()
        {
            
            ClearInventoryButtons();

            int emptyButtons = buttonItem.Length - inventory.inventory.itemList.Count;
            int startingI    = buttonItem.Length - emptyButtons;

            for (int i = 0; i < inventory.inventory.itemList.Count; i++)
            {
                buttonItem[i].GetComponent<InventoryButton>().item = inventory.inventory.itemList[i];

                if (inventory.inventory.itemList[i].IsWeapon()) //This will set an int for the gun going into storages loaded amount
                {  
                    foreach (AmmoInventory.AmmoEntry ammo in ammoInventory._inventory) // cylces through all ammo weapons
                    {
                        if (inventory.inventory.itemList[i].itemType == ammo.weaponType) // finds weapons matching
                        {
                            buttonItem[i].GetComponent<InventoryButton>().loadedAmmo = ammo.loaded; // gets how much that current players has in weapon
                        }
                    }
                }
            }

            for (int i = startingI; i < inventory.inventory.itemList.Count; i++)
            {
                buttonItem[i].GetComponent<InventoryButton>().item = new Item { itemType = Item.ItemType.None, amount = 0 };
            }

            foreach (GameObject item in buttonItem)
            {
                Item inventoryItem = item.GetComponent<InventoryButton>().item;
                Image icon = item.GetComponent<Image>();
                Text amount = item.GetComponentInChildren<Text>();

                icon.sprite = inventoryItem.GetSprite();
                if (inventoryItem.amount > 1) amount.text = inventoryItem.amount.ToString();
                else amount.text = "";
            }

            ammoInventory.SetInventoryAmmo(); //Adjusts AmmoInventory When Adding or Removing Ammo;
        }

        private void ClearInventoryButtons()
        {
            foreach (GameObject item in buttonItem)
            {
                item.GetComponent<InventoryButton>().item = new Item { itemType = Item.ItemType.None, amount = 0 };
            }
        }

        public void OnClick_ButtonClear()
        {
            if (!this.isActiveAndEnabled || item.itemType == Item.ItemType.None) return;
            SoundManagement.Instance.MenuSound(MenuSounds.Cancel);
            ButtonEnabler(true);
            item = new Item { itemType = Item.ItemType.None, amount = 0 };

        }

        void ButtonEnabler(bool isEnabled)
        {
            if(!this.enabled)return;

            foreach(Button button in buttons)
            {
                button.enabled = isEnabled;
            }
        }


    }
}
