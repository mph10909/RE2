using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : DisplayText, IInteractable
{

    [Header("Door Properties")]
    [SerializeField] Transform cameraLocation;
    [SerializeField] AudioClip newMusic;
    [SerializeField] Transform warpLocation;


    [HideInInspector] public GameObject character;
    [HideInInspector] public GameObject characterCamera;
    [Header("Door Interactions")]

    PlayerInventory inventory;

    public GameObject doorToActivate;
    public bool Enter;
    public bool Exit;

    public PlayerInventory Inventory { get {return inventory; }}

    public AudioClip NewMusic
    {
        get { return newMusic; }
        set { newMusic = value;}
    }

    void OnEnable() { Actions.CharacterSwap += SetCharacter;}

    void OnDisable() {Actions.CharacterSwap -= SetCharacter;}

    public delegate void DoorAnimationEventHandler();
    public event DoorAnimationEventHandler OnDoorAnimationComplete;

    private bool isAnimationFinished = false;

    public bool IsAnimationFinished
    {
        get { return isAnimationFinished; }
    }

    public virtual void Interact()
    {
        OpenDoor(); 
    }

    public virtual void OpenDoor()
    {
            Actions.EnemyForget?.Invoke();
            if (Enter) DoorEnter();
            if (Exit) DoorExit();
            if (warpLocation != null) SetCharacter();
            else print("No Warp Location Assigned");
            if(cameraLocation != null) SetCamera();
            else print("No Camera Location Assigned");
    }

    public void SetCamera()
    {
        characterCamera = GameObject.FindGameObjectWithTag("MainCamera");
        characterCamera.transform.position = cameraLocation.position;
        characterCamera.transform.rotation = cameraLocation.rotation;
    }

    public void SetCharacter()
    {
        NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
        agent.Warp(warpLocation.position);
    }

    public void DoorEnter()
    { 
        if(doorToActivate != null)
        {
            GameObject door = Instantiate(doorToActivate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            door.GetComponent<IOpenable>().Open(newMusic);
        }
    }

    public void DoorExit()
    {
        if (doorToActivate != null)
        {
            GameObject door = Instantiate(doorToActivate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            door.GetComponent<IOpenable>().Close(newMusic);
        }
    }

    void SetCharacter(GameObject newCharacter)
    {
        character = newCharacter;
        inventory = character.GetComponent<PlayerInventory>();
    }

}
