using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
   [Range(-2f, 2f)][SerializeField] float xRotation = 0;
   [Range(-2f, 2f)][SerializeField] float yRotation = 0;
   [Range(-2f, 2f)][SerializeField] float zRotation = 0;
   [SerializeField] bool isRotating = true;
    
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0) { if (isRotating) { transform.Rotate(xRotation, yRotation, zRotation); } }
        

        
        if (isRotating) { transform.Rotate(xRotation, yRotation, zRotation); }
    }

}
