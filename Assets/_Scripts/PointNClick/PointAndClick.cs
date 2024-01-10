using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;
using UnityEngine.InputSystem;
using System;

public class PlayerFinishedLookingAtClickable : BaseMessage { }

public interface IInteractor { }

public class PointAndClick : MonoBehaviour, IControllable, IComponentSavable, IInteractor
{
    public CharacterData character;
    [SerializeField] Collider characterCollider;
    [SerializeField] LayerMask ignoreMask;

    public GameObject destinationMarker;

    bool cannotSetMarker;
    bool isAiming;
    bool isSprinting;

    Animator animator;
    NavMeshAgent navMesh;
    [SerializeField] PlayerAction playerActions;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] ClickableColliderDetector colliderDetector;
    [SerializeField] float rotateSpeed = 150;

    private Clickable currentClickable;

    public PlayerAction PlayerActions { get { return playerActions; } }
    public Animator PlayerAnimator { get { return animator; } set { animator = value; } }
    public NavMeshAgent PlayerNavMesh { get { return navMesh; } }
    public PlayerInventory PlayerInventory { get { return playerInventory; } set { } }

    public bool IsAiming { get { return isAiming; } set { isAiming = value; } }
    public bool IsSprinting { get { return isSprinting; } set { isSprinting = value; } }

    public bool NewAnimationSet { get; private set; }

    private IPlayerInput playerInput;



    void OnEnable()
    {
        Actions.ClickedObject += Enabler;
        //Actions.DestinationLocation += DestinationMarkerSpawn; // Called From ClickableObject 
        playerActions.Player.Enable();
        characterCollider.enabled = true;
    }

    void OnDisable()
    {
        navMesh.isStopped = true;
        navMesh.ResetPath();
        Actions.ClickedObject -= Enabler;
        //Actions.DestinationLocation -= DestinationMarkerSpawn;
        playerActions.Player.Disable();
        characterCollider.enabled = false;
    }

    void Awake()
    {
        SetControls();
    }

    public void SetControls()
    {
        playerInput = GetComponentInParent<IPlayerInput>();
        animator = GetComponentInChildren<Animator>();
        playerActions = new PlayerAction();
        navMesh = GetComponent<NavMeshAgent>();

        PlayerActions.Player.Aiming.performed += ctx => IsAiming = true;
        PlayerActions.Player.Aiming.canceled += ctx => IsAiming = false;
        PlayerActions.Player.Sprint.performed += ctx => IsSprinting = true;
        PlayerActions.Player.Sprint.canceled += ctx => IsSprinting = false;
    }

    void Start()
    {
        navMesh.enabled = true;
    }

    void Update()
    {
        StopMovement();

        if (navMesh.isStopped || Vector3.Distance(navMesh.destination, this.transform.position) <= navMesh.stoppingDistance || Vector3.Distance(navMesh.destination, this.transform.position) <= 0.02f)
        {
            if (destinationMarker != null) destinationMarker.SetActive(false);
        }

        if (Vector3.Distance(transform.position, navMesh.destination) <= navMesh.stoppingDistance)
        {
            if (destinationMarker != null) destinationMarker.SetActive(false);
        }

        if (playerInput !=null && playerInput.IsAttackDown())
        {
            MoveCharacter();
        }

    }

    void SetDestination()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreMask))
        {
            
            Clickable clickable = hit.collider.GetComponent<Clickable>();

            if (clickable != null)
            {
                print("Clickable");
                currentClickable = colliderDetector.currentClickable;
                if (Time.timeScale < 1) return;
                SetDestination(clickable, (clickableObject) => OnArriveAtClickable(clickable));
                return;
            }
            else
            {
                print("Not Clickable");
                if (NewAnimationSet) { NewAnimationSet = false; PlayerAnimator.CrossFadeInFixedTime("Idle", 0.5f); }
                if (Time.timeScale < 1) return;
                StopAllCoroutines();
                MoveToLocation(hit.point, null, null);
            }
        }

    }

    void SetDestination(Clickable destinationTransform, Action<Clickable> onArrivalCallback = null)
    {
        if(IsAtInspectionLocation(this.transform, destinationTransform.clickableData.inspectionLocation))
        {
            destinationTransform.Interact?.Invoke(this);
            return;
        }
        else
        {
            MoveToLocation(destinationTransform.clickableData.inspectionLocation.position, destinationTransform, onArrivalCallback);
        }

    }

    bool IsAtInspectionLocation(Transform obj1, Transform obj2)
    {
        Vector3 pos1 = obj1.transform.position;
        Vector3 pos2 = obj2.transform.position;

        return Mathf.Abs(pos1.x - pos2.x) < .1 && Mathf.Abs(pos1.z - pos2.z) < .1;
    }

    void MoveToLocation(Vector3 targetPosition, Clickable data, Action<Clickable> onArrivalCallback = null)
    {
        navMesh.isStopped = false;
        navMesh.SetDestination(targetPosition);

        if (destinationMarker != null)
        {
            DestinationMarkerSpawn(targetPosition);
        }

        if (onArrivalCallback != null)
        {
            StartCoroutine(WaitToReachDestination(data, onArrivalCallback));
        }
    }

    private void DestinationMarkerSpawn(Vector3 markerDestination)
    {
        if (cannotSetMarker) return;
        if (destinationMarker != null)
        {
            destinationMarker.SetActive(true);
            destinationMarker.transform.position = navMesh.destination;
        }
    }

    void Enabler(bool enable)
    {
        cannotSetMarker = enable;
    }

    void NavigationEnd()
    {
        navMesh.isStopped = true;
    }

    void MoveCharacter()
    {
        if (!playerInput.IsAimingHeld())
        {
            if (Cursor.visible == false) return;
            if (navMesh != null) navMesh.ResetPath();
            SetDestination();
        }
    }


    void OnArriveAtClickable(Clickable targetTransform)
    {
        // Make sure this gets called only after arrival.
        if (navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending)
        {
            StartCoroutine(SmoothLookAtCoroutine(targetTransform));
        }
    }

    void StopMovement()
    {
        if (!isAiming) return;
        navMesh.isStopped = true;
        navMesh.ResetPath();
        if (destinationMarker != null) destinationMarker.SetActive(false);
        return;
    }

    public void EnableControl(bool enable)
    {
        this.enabled = enable;
    }

    public string GetSavableData()
    {
        return this.enabled.ToString();
    }

    public void SetFromSaveData(string savedData)
    {
        if (bool.TryParse(savedData, out bool result))
        {
            this.enabled = result;
        }
    }

    IEnumerator WaitToReachDestination(Clickable data, Action<Clickable> onArrivalCallback)
    {

        while (navMesh.pathPending)
        {
            yield return null;
        }

        yield return new WaitUntil(() => navMesh.velocity.sqrMagnitude > 0f);

        while (navMesh.hasPath && navMesh.pathStatus == NavMeshPathStatus.PathComplete && navMesh.remainingDistance > navMesh.stoppingDistance)
        {
            yield return null; // Wait until the character is close enough to the destination.
        }

        if (navMesh.pathStatus == NavMeshPathStatus.PathComplete && !navMesh.pathPending)
        {
            StartCoroutine(SmoothLookAtCoroutine(data));
        }
    }


    IEnumerator SmoothLookAtCoroutine(Clickable target)
    {
        Vector3 directionToLook = target.clickableData.inspectionLocation.parent.position - transform.position;
        directionToLook.y = 0;
        Quaternion finalLookRotation = Quaternion.LookRotation(directionToLook);

        while (Quaternion.Angle(transform.rotation, finalLookRotation) > 1f)
        {
            float step = rotateSpeed * Time.unscaledDeltaTime; // Changed to use scaled time to be consistent with game time scale.
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.RotateTowards(currentRotation, finalLookRotation, step);
            yield return null;
        }

        if (target.clickableData.clickData != null)
        PlayerAnimator.CrossFadeInFixedTime(target.clickableData.clickData.Animation, 0.5f);
        NewAnimationSet = true;
        
        target.Interact?.Invoke(this);
    }







}
