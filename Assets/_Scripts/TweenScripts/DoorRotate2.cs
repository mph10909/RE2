using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotate2 : TweenData
 {   
    
    public Vector3 startRot;
    public Vector3 targetRot;
    
    

    public override void Update()
    {
        base.Update();
        float percent = elapseDuration / totalDuration;
        transform.localEulerAngles = Vector3.Lerp(targetRot, startRot, percent);
        
    }


 }