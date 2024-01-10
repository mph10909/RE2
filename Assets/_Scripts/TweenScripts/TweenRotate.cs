using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotate : TweenDataItems
 {   
    
    public Transform startRot;
    public Transform targetRot;
    public static Quaternion lookRotation;
    

    public override void Update()
    {
        base.Update();
        lookRotation = Quaternion.LookRotation(targetRot.transform.position - startRot.transform.position);
        float percent = elapseDuration / totalDuration;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, percent);
        
    }


 }