using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
Vector3 floatY;
float originalY;
public float speed;
public float floatStrength;

void Start ()
{
    this.originalY = this.transform.position.y;
}

void Update () {
    transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time * speed) * floatStrength));
}
}
