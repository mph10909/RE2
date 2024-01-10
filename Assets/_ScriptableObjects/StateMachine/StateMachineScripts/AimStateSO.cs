using UnityEngine;
using System.Collections;
[CreateAssetMenu(menuName = "FSM/States/AimState")]
public class AimStateSO : StateSO
{

    public StateSO runState;
    public StateSO followState;
    public StateSO idleState;
    public StateSO fireState;
    public StateSO evadeState;

    public float someRotationSpeed = 5;
    public float aimStateOff = 1;

    public override void EnterState(FSMController controller)
    {
        controller.navMeshAgent.SetDestination(controller.transform.position);
        controller.characterController.PlayerAnimator.SetBool(FSMController.AIMER, true);
    }

    public override void ExecuteState(FSMController controller)
    {
        if (controller.enemiesInRange.Count == 0) return;
        if (controller.enemiesInRange.Count == 1)
        {
            controller.currentTarget = controller.enemiesInRange[0];
        }
        else if (controller.enemiesInRange.Count > 1)
        {
            controller.currentTarget = GetClosestEnemy(controller);
        }
        FaceEnemy(controller);
    }


    public override void ExitState(FSMController controller)
    {
    }

    public override void CheckTransitions(FSMController controller)
    {
        //Debug.Log("Enemies Close " + controller.IsAnyEnemyTooClose());

        if (controller.IsAnyEnemyTooClose())
        {
            controller.StopAim();
            controller.SetState(evadeState);
            return;
        }

        if (controller.IsWeaponIsEmpty())
        {
            controller.StopAim();
            //controller.StateDelay(followState, aimStateOff);
            controller.SetState(followState);
            return;
        }
        if (controller.enemiesInRange.Count == 0)
        {
            controller.currentTarget = controller.player;
            controller.StopAim();
            controller.SetState(idleState);
            //controller.StateDelay(followState, aimStateOff);
            return;
        }

        if (controller.distanceToPlayer > controller.startRunning)
        {
            controller.StopAim();
            controller.SetState(followState);
            //controller.StateDelay(followState, aimStateOff);
            return;
        }

        if (CanSeeTarget(controller.currentTarget, controller) && controller.characterController.PlayerAnimator.GetBool(FSMController.AIMING) && !controller.characterController.PlayerAnimator.GetBool(FSMController.FIRING))
        {
            controller.SetState(fireState);
        }
    }

    private void FaceEnemy(FSMController controller)
    {
        Vector3 directionToTarget = (controller.currentTarget.position - controller.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * someRotationSpeed);
    }

    private Transform GetClosestEnemy(FSMController controller)
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in controller.enemiesInRange)
        {
            if (enemy != null)
            {
                float currentDistance = Vector3.Distance(controller.transform.position, enemy.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private bool CanSeeTarget(Transform target, FSMController controller)
    {
        if (target == null) return false;

        RaycastHit hit;
        Vector3 rayDirection = controller.transform.forward; // This is the forward direction of the character.
        Vector3 rayOrigin = controller.transform.position + Vector3.up * 1.5f;

        // Draw a ray for visualization in the editor
        Debug.DrawRay(rayOrigin, rayDirection * 100f, Color.red, 0.1f); // Ray will be red and last for 0.1 second

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 100f)) // Assuming a maximum raycast distance of 100 units.
        {
            return hit.transform == target;
        }

        return false;
    }

}
