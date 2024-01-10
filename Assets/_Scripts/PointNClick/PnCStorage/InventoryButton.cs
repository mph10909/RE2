using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class InventoryButton : MonoBehaviour
    {
        public Item item;
        [SerializeField] GameObject itemSurround;
        [SerializeField] SetInventoryButtons inventory;
        public int loadedAmmo;

        void OnEnable()
        {
            ItemStorageManager.SurroundOff += SurroundOff;
        }

        void OnDisable()
        {
            ItemStorageManager.SurroundOff -= SurroundOff;
            SurroundOff();
        }

        public void OnClick_InventoryButton()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            ItemStorageManager.ButtonEnable?.Invoke(false);     //Disables All Button In SetInventoryButtons Until Canceled or Storage Selected 
            ItemStorageManager.SurroundOff?.Invoke();           //Disables Any Button Surrounds
            if (item.itemType == Item.ItemType.None) { NoItemButton(); return; }
            if (item.IsStackable() ) inventory.item = new Item {itemType = item.itemType, amount = item.amount };
            else inventory.item = item;
            inventory.ammo = loadedAmmo;
            TransferringActive();
            return;
        }

        void NoItemButton()
        {
            TransferringActive();
            ItemStorageManager.MovingItemToInventory?.Invoke(true);
        }

        void TransferringActive()
        {
            ItemStorageManager.Scrolling?.Invoke(true);
            itemSurround.SetActive(true);
        }

        void SurroundOff()
        {
            itemSurround.SetActive(false);
        }
    }
}
