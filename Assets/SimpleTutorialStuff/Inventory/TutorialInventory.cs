using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResidentEvilClone
{
    [Serializable]
    public class TutorialInventory
    {
        public List<TutorialItem> itemList; // List to store the items in the inventory
        TutorialItem.TutItem item;

        public TutorialInventory()
        {
            itemList = new List<TutorialItem>(); // Initializing the itemList
        }

        // Adds the item to the itemList
        public void AddItem(TutorialItem item)
        {
            itemList.Add(item);
        }

        // Removes the item from the itemList
        public void RemoveItem(TutorialItem item)
        {
            Debug.Log("Remove " + item.item.ToString());
            itemList.Remove(item);
        }

        // Returns the list of items in the inventory
        public List<TutorialItem> GetItems()
        {
            return itemList;
        }

        // Returns the item in the inventory
        public TutorialItem.TutItem GetItem()
        {
            return item;
        }

        // Returns true if the inventory contains the specified item, false otherwise
        public bool ContainsItem(TutorialItem.TutItem item)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Debug.Log(itemList[i].item.ToString()); // Output the current item in the list
                if (itemList[i].item == item) return true; // Check if the item matches the specified item
            }
            return false; // The item is not found in the inventory
        }

    }
}

