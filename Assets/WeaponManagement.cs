using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManagement : MonoBehaviour
{
    public SO_Inventory playerInventory;

    void Awake()
    {
        LoadWeaponData();
    }

    private void LoadWeaponData()
    {
        foreach (InventoryObject weapon in playerInventory.items)
        {
            if (weapon.inventoryItem is WeaponItem weaponItem)
        {
                bool isWeaponLoaded = false;

                // Check if the weapon is already loaded.
                foreach (AmmoObject ammoObject in playerInventory.weaponLoaded)
                {
                    if (ammoObject._weapon == weaponItem)
                    {
                        isWeaponLoaded = true;
                        break;
                    }
                }

                if (!isWeaponLoaded)
                {
                    // Create an AmmoObject for the weapon and add it to the weaponLoaded list.
                    playerInventory.weaponLoaded.Add(new AmmoObject
                    {
                        _weapon = weaponItem,
                        _loaded = weaponItem.maxAmmo // Set the initial loaded ammo to the weapon's maximum ammo capacity.
                    });
                }
            }
        }
    }
}

