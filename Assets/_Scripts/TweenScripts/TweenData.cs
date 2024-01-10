using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenData : MonoBehaviour
{
    
    public float elapseDuration;
    public float totalDuration;
    public Action onComplete;

    public virtual void Update()
    {
        elapseDuration += Time.unscaledDeltaTime; // Maybe Unscaled?

        if(elapseDuration >= totalDuration) //End Time
        {   

            enabled = false;
            onComplete?.Invoke();
            ComponentController.inventoryItemsMoving = false;
        }
    }

}
