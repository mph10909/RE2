using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class ClickableObject : MonoBehaviour
{

    PlayerAction playerActions;
    [SerializeField] bool pathSet;
    [SerializeField] float rotateSpeed = 150f;
    Transform destination;

    [SerializeField]GameObject character;
    NavMeshAgent playerNav;
    Transform inspectionLocation;
    Transform target;
    Component[] interactables;
    bool closeEnough;

    [SerializeField]List<GameObject> gameobjects = new List<GameObject>();
    [SerializeField] UnityEvent m_Interact;
    private CharacterManager charManager;

    public bool CloseEnoughToClick { get { return closeEnough; } set { closeEnough = value; } }

    void OnValidate()
    {
        inspectionLocation = this.transform.Find("InspectionLocation");
        interactables = GetComponents(typeof(IInteractable));

        if (!gameobjects.Contains(this.gameObject))
        {
            gameobjects.Add(this.gameObject);
        }

    }

    public GameObject Character
    {
        set { character = value;  playerNav = character.GetComponent<NavMeshAgent>(); }
    }


    void OnEnable()
    {
        Actions.CharacterSwap += SetCharacter;
        Actions.CameraSwap += PathIsFinished;
        Actions.PathIsFinished += PathIsFinished;
    }

    void OnDisable()
    {
        Actions.CharacterSwap -= SetCharacter;
        Actions.CameraSwap -= PathIsFinished;
        Actions.PathIsFinished -= PathIsFinished;
        playerActions.Player.Fire.canceled -= ClickedItem;
    }

    void Awake()
    {
        charManager = FindObjectOfType<CharacterManager>();
        character = charManager.Character;
        playerActions = new PlayerAction();
        Actions.CharacterSwap += SetCharacter;
        Actions.CameraSwap += PathIsFinished;
        playerActions.Player.Enable();
        interactables = GetComponents(typeof(IInteractable));
        target = this.gameObject.transform;
        inspectionLocation = this.transform.Find("InspectionLocation");
        playerActions.Player.Fire.performed += ctx => pathSet = false;
        playerActions.Player.Fire.canceled += ClickedItem;
    }

    void Start()
    {
        playerNav = character.GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)) pathSet = false;

        if (pathComplete() && destination != null || playerNav.velocity == new Vector3(0,0,0)  && destination != null)
        {
            RotateTowards(target);
        }
    }

   

    void OnMouseDown()
    {
        if(!enabled) return;
        Actions.ClickedObject?.Invoke(true);
    }

    void OnMouseUp()
    {
        if (!enabled) return;
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused || Time.timeScale == 0) return;
        destination = inspectionLocation;
        playerNav.destination = destination.position;
        Actions.ClickedObject?.Invoke(false);                       // Enables Point And Click to Set Marker
        Actions.DestinationLocation?.Invoke(playerNav.destination); // Set The DestinationMarker in Point and Click Script
        pathSet = true;
    }

    void RotateTowards (Transform target)
    {
        if (!pathSet) PathIsFinished();
        Vector3 direction = (this.transform.position - character.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        if (rotation == character.transform.rotation)
        {
            foreach (GameObject interactables in gameobjects)
            {
                Actions.PathIsFinished?.Invoke();
                var interacting = interactables.GetComponent<IInteractable>();
                if (interacting != null) interacting.Interact();
            }
        }
    }  

    protected bool pathComplete()
    {
        
        if (Vector3.Distance(playerNav.destination, playerNav.transform.position) <= playerNav.stoppingDistance)
        {
            if (!playerNav.hasPath || playerNav.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    void PathIsFinished()
    {
        destination = null;
        return;
    }

    void SetCharacter(GameObject newCharacter)
    {
        //print("CharacterSet");
        character = newCharacter;
        playerNav = newCharacter.GetComponent<NavMeshAgent>();
    }

    void ClickedItem(InputAction.CallbackContext context)
    {
        if (this == null || !this.gameObject.activeInHierarchy) return;
        if (!CloseEnoughToClick) return;
        if (context.canceled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this object's collider
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Run the desired script when the mouse position is over the collider of this object

                if (!enabled) return;
                if (GameStateManager.Instance.CurrentGameState == GameState.Paused || Time.timeScale == 0) return;

                destination = inspectionLocation;
                playerNav.destination = destination.position;
                Actions.ClickedObject?.Invoke(false);
                Actions.DestinationLocation?.Invoke(playerNav.destination);
                pathSet = true;
            }
        }
        // Perform a raycast from the mouse position

    }
}
