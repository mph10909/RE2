using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class StorageButton : MonoBehaviour
    {
        [SerializeField] SetInventoryButtons inventory;
        [SerializeField] int weaponLoadedAmount;
        public Item storageItem;

        bool movingToInventory;
        bool oneTime;

        Text itemText, amountText;
        Image itemImage;

        void Awake()
        {
            amountText = this.transform.GetChild(2).GetComponent<Text>();
            itemText = GetComponentInChildren<Text>();
            itemImage = GetComponentInChildren<Image>();
        }

        void OnEnable()
        {
            ItemStorageManager.MovingItemToInventory += MovingToInventory;

            if (storageItem.itemType != Item.ItemType.None)
            { 
                itemText.text = storageItem.GetText();
                itemImage.sprite = storageItem.GetSprite();
                itemImage.enabled = true;
            }
        }

        public void OnClick_StorageButton()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            if (movingToInventory) { MoveToInventory(); return; }
            if (storageItem.itemType != Item.ItemType.None) { SwapItems(); return; }
            if (inventory.item.itemType != Item.ItemType.None) { MoveToStorage(); return; }
        }

        void MoveToStorage()
        {
            SetStorageItem();
            
            inventory.ammoInventory.AmmoToStorage(storageItem); // Removes Available Ammo thats not loaded in weapon
            Actions.WeaponToStorage?.Invoke(storageItem);       // Unequips Weapon When Moving it into Inventory in the Weapon Swap Script 
        }

        void SwapItems()
        {
            inventory.inventory.inventory.AddItem(storageItem);
            SetStorageItem();
        }

        void MoveToInventory()
        {
            if (storageItem.IsWeapon() && CheckIfDuplicateWeapon()) { return; }
            if (storageItem.itemType == Item.ItemType.None)  { Cancel(); return; }
            if (storageItem.IsWeapon()) { print(storageItem.IsWeapon()); SetPlayerWeaponAmmo(weaponLoadedAmount, storageItem); }
            itemImage.enabled = false;
            itemText.text = "No Item";
            amountText.text = "";
            inventory.inventory.inventory.AddItem(storageItem);
            storageItem = new Item { itemType = Item.ItemType.None, amount = 0 };
            Cancel();
            
        }

        void SetStorageItem()
        {
            weaponLoadedAmount = inventory.ammo;
            storageItem = inventory.item;
            itemText.text = storageItem.GetText();
            itemImage.sprite = storageItem.GetSprite();

            itemImage.enabled = true;
            RemoveItem();

            if (storageItem.amount > 1) amountText.text = storageItem.amount.ToString();
            else amountText.text = "";

            inventory.item = new Item { itemType = Item.ItemType.None, amount = 0 };
            Cancel();         
        }

        void RemoveItem()
        {
            foreach(Item item in inventory.inventory.inventory.GetItemList())
            {
                if(item.itemType == storageItem.itemType)
                {
                    print(item.itemType);
                    inventory.inventory.inventory.RemoveItem(item);
                    return;
                }
            }
        }

        bool CheckIfDuplicateWeapon()
        {
            foreach(Item weapon in inventory.inventory.inventory.GetItemList())
            {
                if (storageItem.itemType == weapon.itemType)return true;    
            }
            return false;
        }

        void Cancel()
        {
            ItemStorageManager.ButtonEnable?.Invoke(true);              // Enables All Inventory Buttons Again
            ItemStorageManager.Scrolling?.Invoke(false);                // Called On The Storage Scroll Script
            ItemStorageManager.SurroundOff?.Invoke();                   // Called On Inventory Buttons
            ItemStorageManager.MovingItemToInventory?.Invoke(false);    // Disables The Scrolling Of Storage 
        }

        void MovingToInventory(bool moving)
        {
            movingToInventory = moving;
        }

        void SetPlayerWeaponAmmo(int amount, Item weapon)
        {
            for (int i = 0; i < inventory.ammoInventory._inventory.Count; i++)
            {
                AmmoInventory.AmmoEntry ammo = inventory.ammoInventory._inventory[i];
                if (weapon.itemType == ammo.weaponType)
                {
                    ammo.loaded = amount;
                    inventory.ammoInventory._inventory[i] = ammo;
                }
            }
        }
    }
}
