using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTween : MonoBehaviour
{
    [SerializeField] GameObject DoorHandle , DoorHandle2;
    [SerializeField] GameObject Door, Door2;
    [SerializeField] GameObject MainObject;
    [SerializeField] float HowLongToFinish;
    [SerializeField] Vector3 doorHandleOpen;
    [SerializeField] Vector3 doorOpen, doorOpen2;
    

    void Start() {
        //Tween.Fade(FadingtestImage.GetComponent<Image>(), 255, HowLongToFinish, ()=> Tween.Fade(FadingtestImage.GetComponent<Image>(), 0, HowLongToFinish));
    }
    void Update()
    {      
        if(Input.GetKeyDown(KeyCode.C)){Debug.Log(DoorHandle.transform.localEulerAngles);}  
        if(Input.GetKeyDown(KeyCode.X)){
            //MouseLook.mousSensitivity = 0f;
            //TweenTest();
            //if(Door2 !=null && DoorHandle2 !=null)TweenTest2();
            //Tween.Rotation(StartingObject, FinishingObject, HowLongToFinish, () => Finished());
            }
    }

    void Finished()
    {
        //Debug.Log(TweenRotate.lookRotation);
        //StartingObject.transform.rotation = TweenRotate.lookRotation;
    }

    //void TweenTest()
    //{
    //    //Tween.DoorOpen(DoorHandle, doorHandleOpen, 2, 
    //                ()=> Tween.DoorOpen(Door, doorOpen, 3));
    //    if(Door2 == null || DoorHandle2 ==null) return;
    //    //Tween.DoorOpen(DoorHandle2, doorHandleOpen, 2, 
    //                ()=> Tween.DoorOpen(Door2, doorOpen2, 3));
    
    //}
        void TweenTest2()
    {

    }

    

}
