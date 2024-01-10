using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : TweenData
{ 
    public Vector3 startPosition;
    public Vector3 targetPosition;

    public override void Update()
    {
        base.Update();
        
        float percent = elapseDuration / totalDuration;

        transform.position = Vector3.MoveTowards(startPosition, targetPosition, percent);
    }
}
