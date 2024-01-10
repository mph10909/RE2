using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest
{
    public event EventHandler OnItemListChanged;
    public List<Item> itemList;
    private Action<Item> useItemAction;
    public static int ListCount;
    public Item.ItemType itemtype;

    public InventoryTest(Action<Item> useItemAction){
        
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
    }

    public void AddItem(Item item){
        if(item.IsStackable()){
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList){
                if(inventoryItem.itemType == item.itemType){
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if(!itemAlreadyInInventory){
                itemList.Add(item);
                ListCount++;
            }
        }else{
            itemList.Add(item);
            ListCount++;
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item){
        if(item.IsStackable()){
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList){
                if(inventoryItem.itemType == item.itemType){
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if(itemInInventory != null && itemInInventory.amount <=0){
                itemList.Remove(item);
                ListCount--;
            }
        }else{
            Debug.Log(item.itemType);
            itemList.Remove(item);
            ListCount--;
        }
        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item){
        useItemAction(item);
    }

    public List<Item> GetItemList(){
        return itemList;
    }
}


