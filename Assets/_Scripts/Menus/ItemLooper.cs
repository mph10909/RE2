using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLooper : MonoBehaviour, ILoopable
{
    
    [SerializeField] float upperMenuItem;
    [SerializeField] float lowerMenuItem;
    [SerializeField] float thisY;
    [SerializeField] bool ItemUp = false;
    [SerializeField] bool ItemDown = false;

    void Start()
    {
        thisY = Mathf.Round(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y / 10) * 10;
    }

    void Update()
    {

        ItemUp = ComponentController.inventoryItemsMoving;
        
            
        
    }
    public void LoopUp()
    {
        thisY = Mathf.Round(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y / 10) * 10;
        if (thisY > 79)
        {
            this.gameObject.transform.localPosition = new Vector3(this.transform.localPosition.x, UI_inventory._lowerList * 20, 0);
            Debug.Log("ItemUp");
            ItemUp = true;
        }
        else ItemUp = false;
    }

    public void LoopDown()
    {
        thisY = Mathf.Round(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y / 10) * 10;
        if (this.gameObject.transform.localPosition.y < UI_inventory._lowerList * 20)
        {
            Debug.Log("ItemDown");
            ItemDown = true;
        }
        else ItemDown = false;
    }
}
