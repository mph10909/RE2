using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportMove : TransportData
{ 
    public GameObject startPosition;
    public GameObject targetObject;
    public Vector3 targetLocation;
    public Vector3 targetRotation;

    public override void Update()
    {
        base.Update();
        targetRotation = targetObject.transform.localEulerAngles;
        targetLocation = targetObject.transform.position;
        transform.position = targetLocation;
        transform.localEulerAngles = targetRotation;

    }
}
