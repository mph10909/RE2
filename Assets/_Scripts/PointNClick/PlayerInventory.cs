using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;
using System;

public class PlayerInventory : MonoBehaviour, IComponentSavable
{
    public PnCInventory inventory;
    [SerializeField] Item[] startingItems;

    public string GetSavableData()
    {
        List<string> dataList = new List<string>();
        List<Item> items = Items();

        for (int i = 0; i < items.Count; i++)
        {
            dataList.Add($"{items[i].itemType.ToString()};{items[i].amount}");
        }

        return string.Join("|", dataList);
    }


    public void SetFromSaveData(string savedData)
    {
        string[] entryDataList = savedData.Split('|');

        inventory.ClearItems(); // Assuming PnCInventory has a method to clear items

        foreach (string entryData in entryDataList)
        {
            string[] fields = entryData.Split(';');
            if (fields.Length != 2) // We expect two fields for each entry: itemType and amount
            {
                Debug.LogError($"Error in parsing savedData entry: {entryData}");
                continue;
            }

            Item.ItemType itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), fields[0]);
            int amount = int.Parse(fields[1]);

            Item newItem = new Item
            {
                itemType = itemType,
                amount = amount
            };

            inventory.AddItem(newItem);
        }
    }


    public List<Item> Items()
    {
        List<Item> items = new List<Item>();
        foreach (Item item in inventory.itemList)
        {
            items.Add(item);
        }
        return items;
    }

    void Start()
    {
        inventory = new PnCInventory();
        //if (inventoryData == null) return;
        foreach(Item item in startingItems)
        {
            inventory.AddItem(item);
        }
    }




}
