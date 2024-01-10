using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCaster : MonoBehaviour
{
    [SerializeField] InventoryItems Items;
    [SerializeField] Text description;
    [SerializeField] GameObject itemImage;   
    [SerializeField] InventoryItemLook lookItem;
     

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit) && !Items.rotating)
        {
            lookItem.itemToLookAt = hit.transform.gameObject;
            description.text = hit.transform.gameObject.name.ToString();
            itemImage.GetComponent<Image>().sprite = hit.transform.gameObject.GetComponent<Image>().sprite;
            itemImage.SetActive(true);
        }
        else
        {
            itemImage.SetActive(false);
            description.text = "";
        }
    }
}
