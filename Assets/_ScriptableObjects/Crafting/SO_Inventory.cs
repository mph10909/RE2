using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryObject
{
    public CraftingIngredient inventoryItem;
    public int itemQuantity;
}

[System.Serializable]
public class AmmoObject
{
    public WeaponItem _weapon;
    public int _loaded;
}

public class SO_Inventory : MonoBehaviour
{

    public List<InventoryObject> items = new List<InventoryObject>();
    public List<AmmoObject> weaponLoaded = new List<AmmoObject>();

    // Add an ingredient to the inventory.
    public void AddItem(CraftingIngredient newItem, int quantity)
    {
        if (newItem.stackable)
        {
            // Check if a stackable item of the same type is already in the inventory.
            foreach (var inventoryItem in items)
            {
                if (inventoryItem.inventoryItem == newItem)
                {
                    // If found, increase the quantity of the existing item.
                    inventoryItem.itemQuantity += quantity;
                    return; // Exit the method since the item was added.
                }
            }
        }

        // If it's not stackable or no matching item was found, add the item as a new InventoryObject.
        items.Add(new InventoryObject { inventoryItem = newItem, itemQuantity = quantity });
    }

    // Remove an ingredient from the inventory.
    public void RemoveItem(CraftingIngredient item, int quantity)
    {
        if (item.stackable)
        {
            // Find the item in the inventory and decrease its quantity.
            var inventoryItem = items.Find(invItem => invItem.inventoryItem == item);
            if (inventoryItem != null)
            {
                inventoryItem.itemQuantity -= quantity;
                if (inventoryItem.itemQuantity <= 0)
                {
                    // If the quantity becomes zero or less, remove the item from the inventory.
                    items.Remove(inventoryItem);
                }
            }
        }
        else
        {
            // If it's not stackable, remove the item directly without quantity checks.
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].inventoryItem == item)
                {
                    items.RemoveAt(i);
                    break; // Remove only one instance of the item.
                }
            }
        }
    }

    public void ReloadWeapon(WeaponItem weapon, int ammoAount)
    {
        foreach (var weaponObject in weaponLoaded)
        {
            if (weaponObject._weapon == weapon)
            {
                int remainingCapacity = weapon.maxAmmo - weaponObject._loaded;

                if (ammoAount >= remainingCapacity)
                {
                    weaponObject._loaded += remainingCapacity;
                }
                else
                {
                    weaponObject._loaded += ammoAount;
                }
                break;
            }
        }
    }

}
