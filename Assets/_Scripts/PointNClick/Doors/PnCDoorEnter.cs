using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

[RequireComponent(typeof(AudioSource))]

public class PnCDoorEnter : MonoBehaviour, IOpenable
{
    AudioSource audioSource;

    Transform door;
    Transform doorHandle;

    IEnumerator handleTurn;
    IEnumerator doorOpen;
    IEnumerator cameraMove;

    GameObject doorCamera;

    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject exitCamera;

    [SerializeField] AudioClip handleTurnFX;
    [SerializeField] AudioClip doorOpenFX;
    [SerializeField] AudioClip doorCloseFx;
    [SerializeField] AudioClip audioToplay;

    [SerializeField] Vector3 handleTurnAmount = new Vector3(-45,0,0);
    [SerializeField] Vector3 doorTurnAmount = new Vector3(0,0,0);

    Transform enterLocation;
    Vector3 doorPosition;
    Vector3 doorRotation;
    Vector3 doorHandleRotation;

    float time = 0;


    void Awake()
    {
        
        DoorComponents();
        DoorStartPosition();
    }

    public void EnableDoor()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
        Actions.DisableCharacterSwap?.Invoke(true);
        DoorComponents();
        StartCoroutine(handleTurn);
    }


    public void ResetDoor()
    {
        time = 0;
        if (doorHandle != null) doorHandle.localEulerAngles = doorHandleRotation;
        //door.SetPosition(doorPosition);
        door.position               = doorPosition;
        door.localEulerAngles       = doorRotation;
    }

    public void DoorComponents()
    {
        door          = transform.Find("Door");
        doorHandle    = transform.Find("Door/Handle");
        audioSource   = GetComponent<AudioSource>();
        handleTurn    = TurnHandle(1);
        doorOpen      = OpenDoor();
        cameraMove    = MoveCamera(); 
    } 

    public void DoorStartPosition()
    {
        
        //doorPosition.GetPosition(door);
        doorPosition = door.position;
        doorRotation = door.localEulerAngles;
        if (doorHandle != null) doorHandleRotation = doorHandle.localEulerAngles;
    }

    //IOpenable Interface//
    public void Open(AudioClip clip)
    {
        audioToplay = clip;
        Debug.Log("OpenDoor");
        this.enabled = true;
        doorCamera    = enterCamera;
        enterLocation = transform.Find("EnterPosition");
        EnableDoor();
    }

    public void Close(AudioClip clip)
    {
        audioToplay = clip;
        Debug.Log("ClosedDoor");
        this.enabled = true;
        doorCamera    = exitCamera;
        enterLocation = transform.Find("ExitPosition");
        EnableDoor();
    }
    //End Inteface//

    private IEnumerator TurnHandle(float waitTime)
    {
        Time.timeScale = 0;
        Cursor.visible = false;
        doorCamera.SetActive(true);
        if (doorHandle != null)
        {        
            time = 0;
            yield return new WaitForSecondsRealtime(waitTime);
            audioSource.PlayOneShot(handleTurnFX);
            Vector3 handleRotation = doorHandle.localEulerAngles;
            audioSource.PlayOneShot(doorOpenFX);
            while (time < 2)
            {
               doorHandle.localEulerAngles = Vector3.Lerp(handleRotation, handleTurnAmount, time / 1f);
               time += Time.unscaledDeltaTime;
               yield return null;
            }
        }

        StartCoroutine(doorOpen); 
    }

    private IEnumerator OpenDoor()
    {
        Vector3 doorRotation = door.localEulerAngles;
        time = 0;
        audioSource.PlayOneShot(doorOpenFX);
        while (time < 3)
        {
            door.localEulerAngles = Vector3.Lerp(doorRotation, doorTurnAmount, time / 2f);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
          
        StartCoroutine(cameraMove);
    }

    private IEnumerator MoveCamera()
    {
        time = 0;
        while (time < 2.5)
        {
            door.position = Vector3.Lerp(door.position, enterLocation.position, time / 75);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        door.transform.position = enterLocation.position;
        audioSource.PlayOneShot(doorCloseFx);
        if (audioToplay != null) { SoundManagement.Instance.SwapTrack(doorCloseFx.length/2, audioToplay); }         
        yield return new WaitForSecondsRealtime(doorCloseFx.length);
        doorCamera.SetActive(false);
        Cursor.visible = true;
        Destroy(this.gameObject);
        this.enabled = false;
    }



    void OnDisable()
    {
        Time.timeScale = 1;
        GameStateManager.Instance.SetState(GameState.GamePlay);
        Actions.DisableCharacterSwap?.Invoke(false);
        ResetDoor();
        StopAllCoroutines();
    }


}
