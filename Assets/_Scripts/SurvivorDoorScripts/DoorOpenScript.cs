using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpenScript : MonoBehaviour
{
    [SerializeField] GameObject DoorCamera;
    [SerializeField] GameObject DoorHandle , DoorHandle2;
    [SerializeField] GameObject Door, Door2;
    [SerializeField] GameObject FadeToDoor;
    [SerializeField] Transform CameraLocation, CameraStartPosition;    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] Vector3 doorHandleOpen;
    [SerializeField] Vector3 doorOpen, doorOpen2;
    public static bool timeForCharacterTransition = false;



    public void Awake(){
        DoorCamera.SetActive(true);
        Inventory.gameIsPaused = true;
        Inventory.PauseGame();
        //Tween.Fade(FadeToDoor.GetComponent<Image>(), 0, 1f, ()=>TweenTest());
    }


    public void TweenTest(){
        audioSource.ignoreListenerPause = true;
        audioSource.PlayOneShot(audioClips[0]);
            if (DoorHandle == null)
            {
                //Tween.DoorOpen(Door, doorOpen, 2,
                //    () => Tween.Move(DoorCamera, CameraLocation.position, 3,
                //    () => ReturnBackToCharacter()));
            }

            if (DoorHandle != null)
            {
                //Tween.DoorOpen(DoorHandle, doorHandleOpen, 2,
                //    () => Tween.DoorOpen(Door, doorOpen, 2,
                //    () => Tween.Move(DoorCamera, CameraLocation.position, 3,
                //    () => ReturnBackToCharacter())));
            }
            
        if(Door2 == null || DoorHandle2 ==null) return;
        //Tween.DoorOpen(DoorHandle2, doorHandleOpen, 2, 
        //            ()=> Tween.DoorOpen(Door2, doorOpen2, 2));

    }

    public void ReturnBackToCharacter(){
        audioSource.ignoreListenerPause = true;
        audioSource.PlayOneShot(audioClips[1]);
        //Tween.Fade(FadeToDoor.GetComponent<Image>(), 1, 2f, ()=> CameraOn());


    }

    public void CameraOn(){
        DoorCamera.SetActive(false);
        //Tween.Fade(FadeToDoor.GetComponent<Image>(), 0, 2f, ()=> itemUnPause());
        
    }
    void itemUnPause(){
        ComponentController.interactingWithDoor = false;
        Inventory.gamePauser = false;
        Inventory.gameIsPaused = false;
        Inventory.PauseGame();
        Destroy(this.gameObject);
    }
}

