using UnityEngine;
using System.Collections;
[CreateAssetMenu(menuName = "FSM/States/IdleState")]
public class IdleStateSO : StateSO
{

    public StateSO runState;
    public StateSO followState;
    public StateSO aimState;
    public StateSO evadeState;
    public float delayBeforeFollowing = 1.5f; // Delay in seconds

    public override void EnterState(FSMController controller)
    {
        controller.StopAim();
        controller.navMeshAgent.SetDestination(controller.transform.position);
        controller.characterController.IsSprinting = false;
    }

    public override void ExecuteState(FSMController controller)
    {

    }

    public override void ExitState(FSMController controller)
    {
    }

    public override void CheckTransitions(FSMController controller)
    {
        if (controller.IsAnyEnemyTooClose())
        {
            controller.SetState(evadeState);
            return;
        }

        if (controller.enemiesInRange.Count > 0 && !controller.IsWeaponIsEmpty())
        {
            controller.SetState(aimState);
            return;
        }

        if (controller.distanceToPlayer > controller.idleDistance + 0.5f)
        {
            controller.StateDelay(followState, delayBeforeFollowing);
        }
    }
}
