using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsage : MonoBehaviour
{
    [SerializeField] private Item.ItemType itemType;

    public Item.ItemType GetItemType(){
        return itemType;
    }

    public void UseItem(){
        gameObject.SetActive(false);
    }
}
