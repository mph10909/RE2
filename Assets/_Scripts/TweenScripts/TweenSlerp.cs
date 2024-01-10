using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenSlerp : TweenData
{ 
    public Transform startPosition;
    public Transform targetPosition;
    
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    public override void Update()
    {   
        base.Update();
        GetCenter(Vector3.up);
        float percent = elapseDuration / totalDuration;
        transform.position = Vector3.Slerp(startRelCenter, endRelCenter, percent);
        transform.position += centerPoint;
    }
    public void GetCenter(Vector3 direction) {
        centerPoint = (startPosition.position + targetPosition.position) * 0.5f;
        centerPoint -= direction;
        startRelCenter = startPosition.position - centerPoint;
        endRelCenter = targetPosition.position - centerPoint;
        
    }

}
