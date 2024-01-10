using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickedUp : TweenDataItems
 {   
    
    public Quaternion startRot;
    public Quaternion targetRot;

    public override void Update()
    {
        base.Update();
        float percent = elapseDuration / totalDuration;
        //transform.rotation = Quaternion.Lerp(startRot, targetRot, percent);
        transform.rotation = Quaternion.Lerp(startRot, targetRot, percent);
        
    }


 }