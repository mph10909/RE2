using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : TweenData
{ 
    public Vector3 startScale;
    //public Transform targetScale;
    public float howMuchScale;

    public override void Update()
    {
        base.Update();
        
        float percent = elapseDuration / totalDuration;

        transform.localScale = Vector3.Lerp(startScale, new Vector3(startScale.x * howMuchScale, startScale.y * howMuchScale, 0.01f), percent);
    }
}
