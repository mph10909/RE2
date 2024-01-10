using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

[RequireComponent(typeof(AudioSource))]

public class PnCDoubleDoorEnter : MonoBehaviour, IOpenable
{
    AudioSource audioSource;
    Transform door;
    Transform door2;
    Transform doorHandle;
    Transform doorHandle2;
    

    IEnumerator handleTurn;
    IEnumerator doorOpen;
    IEnumerator cameraMove;

    GameObject doorCamera;

    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject exitCamera;

    [SerializeField] AudioClip handleTurnFX;
    [SerializeField] AudioClip doorOpenFX;
    [SerializeField] AudioClip doorCloseFx;
    AudioClip audioToplay;

    [Header("First Door Turn Amount")]
    [SerializeField] Vector3 handleTurnAmount = new Vector3(-45, 0, 0);
    [SerializeField] Vector3 doorTurnAmount = new Vector3(0, 0, 0);

    [Header("Second Door Turn Amount")]
    [SerializeField] Vector3 handleTurnAmount2 = new Vector3(-45, 0, 0);
    [SerializeField] Vector3 doorTurnAmount2 = new Vector3(0, 0, 0);

    Transform enterLocation;
    Transform enterLocation2;

    Vector3 doorPosition;
    Vector3 doorPosition2;

    Vector3 doorRotation;
    Vector3 doorRotation2;

    Vector3 doorHandleRotation;
    Vector3 doorHandleRotation2;  

    public float time = 0;

    void Awake()
    {
        DoorComponents();
        DoorStartPosition();
    }

    public void EnableDoor()
    {
        Time.timeScale = 0;
        GameStateManager.Instance.SetState(GameState.Paused);
        Actions.DisableCharacterSwap?.Invoke(true);
        DoorComponents();
        StartCoroutine(handleTurn);     
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        GameStateManager.Instance.SetState(GameState.GamePlay);
        Actions.DisableCharacterSwap?.Invoke(false);
        ResetDoor();
        StopAllCoroutines();
    }

    public void ResetDoor()
    {
        time = 0;

        //Reset Starting Positions
        //Door1
        if (doorHandle != null) doorHandle.localEulerAngles = doorHandleRotation;
        door.position = doorPosition;
        door.localEulerAngles = doorRotation;

        //Door1
        if (doorHandle2 != null) doorHandle2.localEulerAngles = doorHandleRotation;
        door2.position = doorPosition2;
        door2.localEulerAngles = doorRotation2;
    }

    public void DoorComponents()
    {
        //First Door Components
        door = transform.Find("Door");
        doorHandle = transform.Find("Door/Handle");

        //Second Door Components
        door2 = transform.Find("Door2");
        doorHandle2 = transform.Find("Door2/Handle");

        audioSource = GetComponent<AudioSource>();
        handleTurn = TurnHandle(1);
        doorOpen = OpenDoor();
        cameraMove = MoveCamera();
    }

    public void DoorStartPosition()
    {
        //Store Starting Positions
        //Door1
        doorPosition = door.position;
        doorRotation = door.localEulerAngles;

        //Door2
        doorPosition2 = door2.position;
        doorRotation2 = door2.localEulerAngles;

        if (doorHandle != null) doorHandleRotation = doorHandle.localEulerAngles;
        if (doorHandle2 != null) doorHandleRotation2 = doorHandle2.localEulerAngles;

    }

    public void Open(AudioClip clip)
    {
        audioToplay = clip;
        Debug.Log("OpenDoor");
        this.enabled = true;
        doorCamera = enterCamera;
        enterLocation = transform.Find("Positions/EnterPosition");
        enterLocation2 = transform.Find("Positions/EnterPosition2");
        EnableDoor();
    }

    public void Close(AudioClip clip)
    {
        audioToplay = clip;
        Debug.Log("ClosedDoor");
        this.enabled = true;
        doorCamera = exitCamera;
        enterLocation = transform.Find("Positions/ExitPosition");
        enterLocation2 = transform.Find("Positions/ExitPosition2");
        EnableDoor();
    }

    private IEnumerator TurnHandle(float waitTime)
    {
        Time.timeScale = 0;
        Cursor.visible = false;
        doorCamera.SetActive(true);
        yield return new WaitForSecondsRealtime(waitTime);
        if (doorHandle != null)
        { 
            time = 0;
            audioSource.PlayOneShot(handleTurnFX);
            Vector3 handleRotation = doorHandle.localEulerAngles;
            Vector3 handleRotation2 = doorHandle2.localEulerAngles;
            audioSource.PlayOneShot(doorOpenFX);
            while (time < 2)
            {
                doorHandle.localEulerAngles = Vector3.Lerp(handleRotation, handleTurnAmount, time / 1f);
                doorHandle2.localEulerAngles = Vector3.Lerp(handleRotation2, handleTurnAmount2, time / 1f);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        StartCoroutine(doorOpen);
    }

    private IEnumerator OpenDoor()
    {
        Vector3 doorRotation = door.localEulerAngles;
        Vector3 doorRotation2 = door2.localEulerAngles;
        time = 0;
        audioSource.PlayOneShot(doorOpenFX);
        while (time < 3)
        {
            door.localEulerAngles = Vector3.Lerp(doorRotation, doorTurnAmount, time / 2f);
            door2.localEulerAngles = Vector3.Lerp(doorRotation2, doorTurnAmount2, time / 2f);
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
            door2.position = Vector3.Lerp(door2.position, enterLocation2.position, time / 75);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        door.transform.position = enterLocation.position;
        door2.transform.position = enterLocation2.position;
        audioSource.PlayOneShot(doorCloseFx);
        if (audioToplay != null) { SoundManagement.Instance.SwapTrack(doorCloseFx.length/2, audioToplay); }
        yield return new WaitForSecondsRealtime(doorCloseFx.length);
        doorCamera.SetActive(false);
        Cursor.visible = true;
        Destroy(this.gameObject);
        this.enabled = false;
        
    }




}

