using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DpadController : MonoBehaviour
{
    private float dpadX;
    private float dpadY;
    private bool leftDpadPressed;
    private bool rightDpadPressed;
    private bool upDpadPressed;
    private bool downDpadPressed;
    private bool currentlyReleased;
    public static bool leftD;
    public static bool rightD;
    public static bool upD;
    public static bool downD;

 
 private void Start(){
  currentlyReleased = true;    
  }
 
  private void Update()
     {
        dpadX = Input.GetAxis("Horizontal");
        dpadY = Input.GetAxis("Vertical");
         
 
 
         if (dpadX == -1)
         {
             leftDpadPressed = true;
             if(leftDpadPressed && currentlyReleased)
             {
                 //Fire events
                leftD = true;
                //print("LEFT");
                
             }else{
                 leftD = false;
             }
             
 
             currentlyReleased = false;
             
         }
         if(dpadX == 1)
         {
             rightDpadPressed = true;
             if(rightDpadPressed && currentlyReleased)
             {
                 //Fire events
                 rightD = true;
                 //print("RIGHT");
             }
             currentlyReleased = false;
         }
         if (dpadY == -1)
         {
             downDpadPressed = true;
             if (downDpadPressed && currentlyReleased)
             {
                 //Fire events
                 downD = true;
                 //print("DOWN");
             }
             currentlyReleased = false;
         }
         if (dpadY == 1)
         {
             upDpadPressed = true;
             if (upDpadPressed && currentlyReleased)
             {
                 //Fire events
                 upD = true;
                 //print("UP");
             }
             currentlyReleased = false;
         }
         if (dpadY == 0 && dpadX == 0)
         {
            leftD = false;
            rightD = false;
            upD = false;
            downD = false;
            upDpadPressed = false;
            downDpadPressed = false;
            leftDpadPressed = false;
            rightDpadPressed = false;
             currentlyReleased = true;
             
         }
 
     }
 
 
}
