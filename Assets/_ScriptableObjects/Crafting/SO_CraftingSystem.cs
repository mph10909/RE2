using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CraftingSystem : MonoBehaviour
{
    public SO_Inventory playerInventory;
    public Text resultText;
    public Button buttonPrefab;
    public Transform inventoryButtons;


    public RecipeManager recipeManager;

    List<CraftingIngredient> selectedIngredients = new List<CraftingIngredient>();

    void Start()
    { 
        UpdateInventoryButtons();
        
    }
    public void SelectInventoryItem(CraftingIngredient item, Button button)
    {
            selectedIngredients.Add(item);
            button.interactable = false;
            Craft();            
    }

    public void Craft()
    {
        if (selectedIngredients.Count == 2) // Check if two ingredients are selected.
        {
            bool isCraftSuccessful = false;

            foreach (var recipe in recipeManager.craftingRecipes)
            {
                // Check if the selected ingredients match the recipe's ingredients in any order.
                if ((selectedIngredients[0] == recipe.ingredients[0].item && selectedIngredients[1] == recipe.ingredients[1].item) ||
                    (selectedIngredients[0] == recipe.ingredients[1].item && selectedIngredients[1] == recipe.ingredients[0].item))
                {
                    // Check if one of the items is a weapon item.
                    if (selectedIngredients[0] is WeaponItem || selectedIngredients[1] is WeaponItem)
                    {
                        // Check if the other item is ammo.
                        CraftingIngredient ammo = selectedIngredients[0] is WeaponItem ? selectedIngredients[1] : selectedIngredients[0];
                        if (ammo is AmmoItem)
                        {
                            // Perform the reload action here.
                            WeaponItem weapon = selectedIngredients[0] is WeaponItem ? (WeaponItem)selectedIngredients[0] : (WeaponItem)selectedIngredients[1];
                            int AmmoAvailable = 0;

                            foreach (var inventoryObject in playerInventory.items)
                            {
                                if(ammo == inventoryObject.inventoryItem)
                                {
                                    AmmoAvailable = inventoryObject.itemQuantity;
                                    break;
                                }
                            }

                            foreach (var ammoObject in playerInventory.weaponLoaded)
                            {
                                if (ammoObject._weapon == weapon)
                                {
                                    int remainingCapacity = weapon.maxAmmo - ammoObject._loaded;

                                    if (AmmoAvailable >= remainingCapacity)
                                    {
                                        ammoObject._loaded = weapon.maxAmmo;
                                        playerInventory.RemoveItem(ammo, remainingCapacity);
                                    }
                                    else
                                    {
                                        ammoObject._loaded += AmmoAvailable;
                                        playerInventory.RemoveItem(ammo, AmmoAvailable);
                                    }

                                    selectedIngredients.Clear();
                                    resultText.text = "Reloaded " + weapon.itemName;

                                    isCraftSuccessful = true;
                                    UpdateInventoryButtons();
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                    // Remove ingredients from the inventory.
                    playerInventory.RemoveItem(selectedIngredients[0], 1);
                    playerInventory.RemoveItem(selectedIngredients[1], 1);

                    // Add the resulting item to the inventory.
                    playerInventory.AddItem(recipe.resultingItem, 1);

                    // Clear the selected ingredients.
                    selectedIngredients.Clear();

                    // Update the UI.
                    resultText.text = "Crafted " + recipe.resultingItem.itemName;

                    isCraftSuccessful = true;
                    UpdateInventoryButtons();
                    break; // Exit the loop if a successful combination is found.
                    }

                }
            }

            if (!isCraftSuccessful)
            {

                resultText.text = "Selected items do not match any recipe.";
                selectedIngredients.Clear();
                UpdateInventoryButtons();
            }
        }
        else
        {
            resultText.text = "Select two items to craft.";
        }
    }

    void CreateInventoryButton(CraftingIngredient item, int Quantity)
    {
        Button inventoryButton = Instantiate(buttonPrefab, inventoryButtons);
        inventoryButton.GetComponent<Image>().sprite = item.icon;
        inventoryButton.onClick.AddListener(() => SelectInventoryItem(item, inventoryButton));
        inventoryButton.gameObject.SetActive(true);

        Text quantityText = inventoryButton.transform.Find("QuantityText").GetComponent<Text>();

        // Check if the item is stackable.
        if (item.stackable)
        {
            quantityText.gameObject.SetActive(true);
            quantityText.text = "x" + Quantity;
        }

        // Check if the item is a weapon and handle it appropriately.
        if (item is WeaponItem weaponItem)
    {
            DisplayWeaponQuantity(weaponItem, quantityText);
        }
    }

    void DisplayWeaponQuantity(WeaponItem weaponItem, Text quantityText)
    {
        // Check if the weapon is in the player's loaded weapons list.
        foreach (var weaponInInventory in playerInventory.weaponLoaded)
        {
            if (weaponItem == weaponInInventory._weapon)
            {
                quantityText.gameObject.SetActive(true);
                quantityText.text = "x" + weaponInInventory._loaded;
                if(weaponInInventory._loaded == 0)
                {
                    quantityText.text = "Reload";
                }
                return;
            }
        }
    }

    public void UpdateInventoryButtons()
    {
        foreach (Transform child in inventoryButtons)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in playerInventory.items)
        {
            CreateInventoryButton(item.inventoryItem, item.itemQuantity);
        }
    }

}

