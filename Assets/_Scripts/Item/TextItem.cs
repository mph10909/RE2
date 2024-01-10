using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextItem : MonoBehaviour, IUsable, IDeactivate
{
    [TextArea]
    [SerializeField] private string worldTextDescription;

    public void ActivateObject(){
        //if (MouseLook.TextRay){
            Inventory.PAUSE();
            TextDisplay.TextObject.alignment = TextAnchor.MiddleLeft;
            TextDisplay.TextObject.text = worldTextDescription;
        //}
    }

    public void DeactiveUsable()
    {
        TextDisplay.TEXT_DISABLER();
        Inventory.UNPAUSE();
    }

}
