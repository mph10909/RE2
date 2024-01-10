using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour 
{
    [TextArea]
    [SerializeField] private string ItemTextDescription;
    public Item item;

    public void SetItem(Item item) {
        {
            this.item = item;
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf(){
        Destroy(gameObject);

    }

    public void initializeTextItem(){
    TextDisplay.TextObject.alignment = TextAnchor.MiddleCenter;
    TextDisplay.TextObject.text = ItemTextDescription;
    }
}
