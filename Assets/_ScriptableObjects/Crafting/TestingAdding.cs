using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingAdding : MonoBehaviour
{
    public CraftingIngredient item;
    public int quantity;
    public SO_Inventory inventory;
    public CraftingSystem craftingSystem;
    public Text text;

    public AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the text field is initialized properly at the start.
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (inventory.items.Count == 6)
            {
                text.text = "Inventory Full";
                return;
            }

            // Add the item to the inventory and update the UI buttons.
            inventory.AddItem(item, quantity);
            craftingSystem.UpdateInventoryButtons();

            // Reset the text field.
            text.text = "";
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            FireWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }
    }

    private void FireWeapon()
    {
        if (item is WeaponItem weaponItem)
        {
            WeaponItem weapon = weaponItem;

            foreach (var ammoObject in inventory.weaponLoaded)
            {
                if (ammoObject._weapon == weapon && ammoObject._loaded > 0)
                {
                    _audioSource.PlayOneShot(weapon.fire);
                    text.text = "Fire " + weapon.itemName;
                    ammoObject._loaded--;
                    craftingSystem.UpdateInventoryButtons();
                    
                    break;
                }
                if(ammoObject._weapon == weapon && ammoObject._loaded == 0)
                {
                    ReloadWeapon();
                }


            }
        }
    }

    private void ReloadWeapon()
    {
        if (item is WeaponItem weaponItem)
        {
            WeaponItem weapon = weaponItem;

            foreach (var ammoObject in inventory.weaponLoaded)
            {
                if (ammoObject._weapon == weapon && ammoObject._loaded != weapon.maxAmmo)
                {
                    int AmmoAvailable = 0;

                    foreach (var inventoryObject in inventory.items)
                    {
                        if (weapon.ammoType == inventoryObject.inventoryItem)
                        {
                            AmmoAvailable = inventoryObject.itemQuantity;
                            
                            break;
                        }
                    }

                    if (AmmoAvailable == 0)
                    {
                        _audioSource.PlayOneShot(weapon.empty);
                    }
                    else
                    {
                        _audioSource.PlayOneShot(weapon.reload);
                    }

                    foreach (var ammoObj in inventory.weaponLoaded)
                    {
                        if (ammoObj._weapon == weapon)
                        {
                            int remainingCapacity = weapon.maxAmmo - ammoObj._loaded;

                            if (AmmoAvailable >= remainingCapacity)
                            {
                                ammoObj._loaded = weapon.maxAmmo;
                                inventory.RemoveItem(weapon.ammoType, remainingCapacity);
                            }
                            else
                            {
                                ammoObj._loaded += AmmoAvailable;
                                inventory.RemoveItem(weapon.ammoType, AmmoAvailable);
                            }
                            text.text = "Reload " + weapon.itemName;
                            craftingSystem.UpdateInventoryButtons();
                            break;
                        }
                    }
                }
            }
        }
    }
}
