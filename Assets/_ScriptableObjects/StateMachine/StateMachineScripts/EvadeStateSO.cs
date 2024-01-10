using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "FSM/States/EvadeState")]
public class EvadeStateSO : StateSO
{
    public StateSO idleState;
    public float evadeDistance = 10f; // Distance the AI will try to maintain from the player.
    public float timeToEvade = 3f;    // Duration AI will spend evading before re-evaluating situation.

    private float evadeEndTime = 0f;

    public override void EnterState(FSMController controller)
    {
        controller.StopAim();
        controller.characterController.IsSprinting = true;

        Transform closestEnemy = GetClosestEnemy(controller);
        if (closestEnemy != null)
        {
            Vector3 evadeDirection = (controller.transform.position - closestEnemy.position).normalized;
            Vector3 evadeDestination = controller.transform.position + evadeDirection * evadeDistance;
            controller.navMeshAgent.SetDestination(evadeDestination);
            evadeEndTime = Time.time + timeToEvade;
        }
        else
        {
            // No close enemies? Maybe revert to another state.
            controller.SetState(idleState);
        }
    }



    public override void ExecuteState(FSMController controller)
    {
        // Optional: You can add more complex logic like zig-zag patterns or random direction changes.
    }

    public override void ExitState(FSMController controller)
    {
        // Any cleanup or reset logic you want.
    }

    public override void CheckTransitions(FSMController controller)
    {
        if (Time.time >= evadeEndTime || controller.navMeshAgent.remainingDistance <= 0.5f) // If evade time is up or if AI reached destination.
        {
            controller.SetState(idleState);
        }

        // Additional transitions based on other criteria can be added here.
    }

    private Transform GetClosestEnemy(FSMController controller)
    {
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (Transform enemy in controller.enemiesInRange)
        {
            float dist = Vector3.Distance(controller.transform.position, enemy.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}
