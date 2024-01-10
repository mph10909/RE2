using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class FireExtinquish : DisplayText, IInteractable
{
    [SerializeField]
    private Item.ItemType item;
    [SerializeField]
    [TextArea(3, 5)]
    string noExtinquisher = "";
    [SerializeField]
    AudioClip extinquish;
    [SerializeField]
    GameObject keyItem;
    GameObject character;
    [SerializeField]
    PlayerInventory inventory;

    void OnEnable()
    {
        Actions.CharacterSwap += SetCharacter;
    }

    void OnDisable()
    {
        Actions.CharacterSwap -= SetCharacter;
    }

    public Item GetKeyItem()
    {
        return new Item { itemType = item };
    }

    public void Interact()
    {
        foreach (Item items in inventory.inventory.GetItemList())
        {
            if (items.itemType == GetKeyItem().itemType)
            {
                print("WaLa");
                CanExtinguish();
                return;
            }
        }
        CantExtinguish();
    }

    void CantExtinguish()
    {
        TextDisplay(noExtinquisher);
    }

    void CanExtinguish()
    {

        inventory.inventory.RemoveItem(GetKeyItem());
        SoundManagement.Instance.PlaySound(extinquish);
        this.gameObject.SetActive(false);
        keyItem.SetActive(true);
    }

    void SetCharacter(GameObject newCharacter)
    {
        //character = newCharacter;
        //inventory = newCharacter.GetComponent<PlayerInventory>();
        //keyItem.GetComponent<ClickableObject>().Character = newCharacter;
        //keyItem.GetComponent<PlayerDetection>().Character = newCharacter;
        //keyItem.GetComponent<PickUpObject>().Character = newCharacter;

    }
}
