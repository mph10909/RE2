using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ResidentEvilClone;

public class TutorialPlayerInventory : MonoBehaviour
{
    // SerializeField attribute makes it possible to see this variable in the Unity Editor
    [SerializeField] TutorialInventory inventory;
    

    void Start()
    {
        // Subscribe to the "ItemToInventory" event in TutorialActionMediator
        TutorialActionMediator.ItemToInventory += AddItem;
    }

    public void RemoveItem(TutorialItem item)
    {

        inventory.RemoveItem(item);
    }

    // Adds an item to the inventory
    public void AddItem(TutorialItem item)
    {
        // Calls the AddItem method from the TutorialInventory object
        inventory.AddItem(item);
    }

    public bool ContainsItem(TutorialItem.TutItem item)
    {
        return inventory.ContainsItem(item);
    }

    public List<TutorialItem> GetItems
    {
        get { return inventory.itemList; }
    }
}
