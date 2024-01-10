using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class FSMController : MonoBehaviour
{

    public Transform player;
    public Transform weaponSlot;
    public Transform currentTarget;
    public float startRunning = 10f;
    public float stopRunning;
    public float idleDistance;

    public float closeDistanceThreshold = 5f;


    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector]public PointAndClick characterController;

    private float someRotationSpeed = 2;

    public List<Transform> enemiesInRange = new List<Transform>(); 

    public const string AIMER = "Aimer";
    public const string AIMING = "Aiming";
    public const string RELOAD = "Reload";
    public const string FIRE = "Fire";
    public const string FIRING = "Firing";

    public StateSO initialState;
    public StateSO currentState;

    public float distanceToPlayer { get { return Vector3.Distance(transform.position, player.position); } }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PointAndClick>();

        SetState(initialState);
    }

    private void Update()
    {
        CleanUpEnemiesInRangeList();
        currentState.CheckTransitions(this);
        currentState.ExecuteState(this);

        if (enemiesInRange.Count == 0) currentTarget = player;
    }

    public void SetState(StateSO newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.EnterState(this);
        }
    }

    public void StopAim()
    {
        characterController.PlayerAnimator.SetBool(AIMER, false);
        characterController.PlayerAnimator.SetBool(AIMING, false);
    }

    public void StateDelay(StateSO state, float waitTime)
    {
        StartCoroutine(StateTimer(state, waitTime));
    }

    IEnumerator StateTimer(StateSO state, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetState(state);
    }

    public void AddEnemyToList(Collider collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(collider.transform))
        {
            enemiesInRange.Add(collider.transform);
        }
    }

    public void RemoveEnemyFromList(Collider collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(collider.transform);
        }
    }

    public bool IsWeaponIsEmpty()
    {
        foreach (Transform child in weaponSlot)
        {
            var fireableWeapon = child.GetComponent<IFireable>();
            if (fireableWeapon != null)
            {
                return fireableWeapon.IsAmmoEmpty();
            }
        }
        return true; // Default to true if no IFireable is found.
    }

    public void CleanUpEnemiesInRangeList()
    {
        // Check for null references (destroyed objects) 
        enemiesInRange.RemoveAll(item => item == null);

        // Check for disabled objects
        enemiesInRange.RemoveAll(item => !item.gameObject.activeSelf);
    }

    public bool IsAnyEnemyTooClose()
    {
        if (enemiesInRange.Count == 0) return false;

        foreach (Transform enemy in enemiesInRange)
        {
            if (Vector3.Distance(transform.position, enemy.position) <= closeDistanceThreshold)
            {
                return true;
            }
        }
        return false;
    }
}
