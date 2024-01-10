using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayerFSM : MonoBehaviour
{
    [System.Serializable]
    private enum State
    {
        Idle,
        Follow,
        Sprint,
        Aim,
        Fire,
        Reload,

    }

    public Transform player;
    public Transform weaponSlot;
    public Transform currentTarget;
    public float startRunning = 10f;
    public float stopRunning;
    public float idleDistance;
    public float delayBeforeFollowing = 1.5f; // Delay in seconds
    [SerializeField] State currentState;

    private NavMeshAgent navMeshAgent;
    private PointAndClick characterController;
    private float someRotationSpeed = 2;

    public float evadeDistance = 5f;
    private Vector3 evadeDestination;

    [SerializeField] List<Transform> enemiesInRange = new List<Transform>();

    #region Constants
    private const string AIMER = "Aimer";
    private const string AIMING = "Aiming";
    private const string RELOAD = "Reload";
    private const string FIRE = "Fire";
    private const string FIRING = "Firing";
    
    #endregion

    public float distanceToPlayer { get { return Vector3.Distance(transform.position, player.position); } }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PointAndClick>();

        if (!characterController)
        {
            Debug.LogError("No PointAndClick component found on this GameObject.");
            enabled = false;
        }

        currentTarget = player;
        SetState(State.Follow);
    }

    private void Update()
    {
        CheckTransitions();
        ExecuteState();
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case State.Follow:
            case State.Sprint:
                if (distanceToPlayer <= idleDistance)
                {
                    SetState(State.Idle);
                }
                else navMeshAgent.SetDestination(player.position);
                break;

            case State.Aim:
                if (currentTarget == null || currentTarget.gameObject == null)
                {
                    // Handle the case here, like switching the state back to following the player.
                    currentTarget = player;
                    StartCoroutine(TransitionOutOfAim(State.Follow));
                    return;
                }

                FaceEnemy();

                if (enemiesInRange.Count > 1)
                {
                    currentTarget = GetClosestEnemy();
                }
                break;

        }

    }

    private void CheckTransitions()
    {
        
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;

        switch (currentState)
        {
            case State.Follow:
                if (distanceToPlayer > startRunning)
                {
                    SetState(State.Sprint);
                }

                break;

            case State.Sprint:
                if (distanceToPlayer <= stopRunning)
                {
                    SetState(State.Follow);
                }
                break;

            case State.Aim:
                KeepFollowingPlayer(playerVelocity);
                if (CanSeeTarget(currentTarget) && characterController.PlayerAnimator.GetBool(AIMING) && !characterController.PlayerAnimator.GetBool(FIRING))
                {
                    SetState(State.Fire);
                }
                break;

            case State.Fire:
                KeepFollowingPlayer(playerVelocity);
                if (characterController.PlayerAnimator.GetBool(FIRING))
                {
                    SetState(State.Aim);
                }
                if (characterController.PlayerAnimator.GetBool(RELOAD))
                {
                    SetState(State.Reload);
                }
                break;
            case State.Reload:
                if (!characterController.PlayerAnimator.GetBool(RELOAD))
                {
                    SetState(State.Aim);
                }
                    break;
        }
    }

    private void SetState(State newState)
    {
        Debug.Log("Transitioning from " + currentState + " to " + newState);
        currentState = newState;

        switch (currentState)
        {
            case State.Idle:
                StopAIm();
                navMeshAgent.SetDestination(transform.position);
                characterController.IsSprinting = false;
                characterController.PlayerAnimator.SetBool(FIRING, false);
                break;

            case State.Follow:
                //StopAIm();
                characterController.IsSprinting = false;
                characterController.PlayerAnimator.SetBool(FIRING, false);
                break;

            case State.Sprint:
                //StopAIm();
                characterController.IsSprinting = true;
                characterController.PlayerAnimator.SetBool(FIRING, false);
                break;

            case State.Aim:
                navMeshAgent.SetDestination(transform.position);
                characterController.PlayerAnimator.SetBool(AIMER, true);
                break;

            case State.Fire:
                FireWeapon();
                break;

            case State.Reload:
                navMeshAgent.SetDestination(transform.position);
                break;
        }
    }

    private void FaceEnemy()
    {
        // If the NPC is in the Aim or Fire state, don't rotate towards the player
        if (currentState == State.Aim || currentState == State.Fire)
        {
            if (currentTarget == player) return;
        }

        if (!currentTarget) return;  // Return early if there's no valid target

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * someRotationSpeed);
    }


    private void KeepFollowingPlayer(Vector3 playerVelocity)
        {
            if (playerVelocity != Vector3.zero) // Player starts moving
            {
                Debug.Log("Player started moving.");
                currentTarget = player;  // Switch attention back to player
                SetState(State.Follow);
            }
        }

    private void FireWeapon()
    {
        if (currentState != State.Fire) return;
        if (!currentTarget || currentTarget == player) return; // Make sure there's a valid target and it's not the player

        foreach (Transform child in weaponSlot)
        {
            var fireableWeapon = child.GetComponent<IFireable>();
            if (fireableWeapon != null)
            {

                if (fireableWeapon.AIFire())
                {
                    characterController.PlayerAnimator.SetTrigger(FIRE);
                    break;
                }
                else if(characterController.PlayerAnimator.GetBool(RELOAD))
                {
                    SetState(State.Reload);
                    break;
                }
                else if(!characterController.PlayerAnimator.GetBool(AIMING))
                {
                    StartCoroutine(TransitionOutOfAim(State.Sprint));
                }
            }
        }
    }

    private void StopAIm()
    {
        characterController.PlayerAnimator.SetBool(AIMER, false);
        characterController.PlayerAnimator.SetBool(AIMING, false);
    }

    public void RemoveEnemyFromList(Transform enemyToRemove)
    {
        if (enemiesInRange.Contains(enemyToRemove))
        {
            enemiesInRange.Remove(enemyToRemove);

            if (currentTarget == enemyToRemove)
            {
                currentTarget = player;
                if (currentState == State.Aim || currentState == State.Fire)
                {
                    SetState(State.Follow); // Or another appropriate state.
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            SetState(State.Idle);
        }
        else if (other.GetComponent<Enemy>() != null && currentState != State.Sprint)
        {
            enemiesInRange.Add(other.transform);

            bool isWeaponEmpty = CheckIfWeaponIsEmpty();

            if (!isWeaponEmpty)
            {
                currentTarget = GetClosestEnemy();
                SetState(State.Aim);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            StartCoroutine(WaitAndFollow());
        }
        else if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Remove(other.transform);

            if (enemiesInRange.Count == 0 || currentTarget == other.transform)
            {
                currentTarget = player;
                if (currentState == State.Aim || currentState == State.Fire || currentState == State.Reload)
                {
                    StartCoroutine(TransitionOutOfAim(State.Follow)); // Switch back to follow if no enemies are in range
                }
            }
            else if (enemiesInRange.Count > 0)
            {
                currentTarget = GetClosestEnemy(); // Optional: If you still have enemies in the list, target the closest one.
            }
        }
    }




    IEnumerator WaitAndFollow()
    {
        yield return new WaitForSeconds(delayBeforeFollowing);
        SetState(State.Follow);
    }

    IEnumerator TransitionOutOfAim(State state)
    {
        StopAIm();
        yield return new WaitForSeconds(1.5f);
        SetState(state);
    }

    private bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        RaycastHit hit;
        Vector3 directionToTarget = target.position - transform.position;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f; // adjust 1.5f as necessary

        // Draw a ray for visualization in the editor
        Debug.DrawRay(rayOrigin, directionToTarget, Color.red, 1f); // Ray will be red and last for 1 second

        if (Physics.Raycast(rayOrigin, directionToTarget, out hit))
        {
            return hit.transform == target;
        }

        return false;
    }

    private bool CheckIfWeaponIsEmpty()
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

    private Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float currentDistance = Vector3.Distance(transform.position, enemy.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private Vector3 GetEvadePosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * evadeDistance;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, evadeDistance, -1);
        return navHit.position;
    }


}
