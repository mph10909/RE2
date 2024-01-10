using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PnCInventory
{

    public List<Item> itemList;

    public PnCInventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        Actions.InventoryItemChange?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }

            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(item);
                Actions.InventoryItemChange?.Invoke();
            }
        }
        else
        {
            itemList.Remove(item);
        }
        
        Actions.InventoryItemChange?.Invoke(); 
    }

    public void RemoveItemType(Item.ItemType itemType)
    {
        Item itemToRemove = null;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == itemType)
            {
                itemToRemove = inventoryItem;
                break;
            }
        }

        if (itemToRemove != null)
        {
            Debug.Log(itemToRemove.itemType);
            if (itemToRemove.IsStackable())
            {
                itemToRemove.amount -= 1;
                if (itemToRemove.amount <= 0)
                {
                    itemList.Remove(itemToRemove);
                }
            }
            else
            {
                itemList.Remove(itemToRemove);
            }

            Actions.InventoryItemChange?.Invoke();
        }
    }

    public void UseItem(Item item)
    {

    }

    public void ClearItems()
    {
        itemList.Clear();
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public Item GetItem(Item.ItemType itemType)
    {
        foreach (Item item in itemList)
        {
            if (item.itemType == itemType)
            {
                return item;
            }
        }
        return null;
    }
}
