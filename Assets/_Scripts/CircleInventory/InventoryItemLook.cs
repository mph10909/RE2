using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemLook : MonoBehaviour
{
    public GameObject itemToLookAt;


    // Update is called once per frame
    void Update()
    {
        if (itemToLookAt == null) return;
        this.transform.LookAt(itemToLookAt.transform);  
    }
}
