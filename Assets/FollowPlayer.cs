using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform.
    public float startRunning = 10f;  // Distance threshold to start running.
    public float stopRunning;  // Distance threshold to stop running.
    public bool isTooFarTrigger = false;  // Trigger that gets activated if player is too far.

    private NavMeshAgent navMeshAgent;
    private bool isPlayerInside = false;
    private PointAndClick characterController;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PointAndClick>();

        // Ensure the PointAndClick component exists on this GameObject.
        if (!characterController)
        {
            Debug.LogError("No PointAndClick component found on this GameObject.");
            enabled = false;  // Disable this script.
        }

        isPlayerInside = false;
        StartCoroutine(FollowTarget());
    }

    private void CheckDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > startRunning)
        {
            if (!isTooFarTrigger)
            {
                isTooFarTrigger = true;
                characterController.IsSprinting = true;
                Debug.Log("Player is too far! Trigger is ON.");
            }
        }
        else if (distanceToPlayer <= stopRunning)
        {
            if (isTooFarTrigger)
            {
                characterController.IsSprinting = false;
                isTooFarTrigger = false;
                Debug.Log("Player is within range. Trigger is OFF.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerInside = true;
            navMeshAgent.SetDestination(transform.position);
            if (isTooFarTrigger)
            {
                isTooFarTrigger = false;
                Debug.Log("Player is within range. Trigger is OFF.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerInside = false;
            StopCoroutine(FollowTarget()); // Stop the coroutine.
            StartCoroutine(FollowTarget());
        }
    }


    IEnumerator FollowTarget()
    {
        while (!isPlayerInside)
        {
            navMeshAgent.SetDestination(player.position);
            yield return new WaitForSeconds(0.1f);  // Wait a bit to let the NavMeshAgent start moving.
            CheckDistance();
            yield return null;  // Wait for the next frame.
        }
    }

}
