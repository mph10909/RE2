using UnityEngine;
[CreateAssetMenu(menuName = "FSM/States/RunState")]
public class RunStateSO : StateSO
{

    public StateSO idleState;
    public StateSO followState;
    private float nextUpdateTime = 0f;
    private float updateInterval = 0.1f;

    public override void EnterState(FSMController controller)
    {
        controller.navMeshAgent.SetDestination(controller.player.position);
        controller.characterController.IsSprinting = true;
        controller.characterController.PlayerAnimator.SetBool(FSMController.FIRING, false);
    }

    public override void ExecuteState(FSMController controller)
    {
        if (controller.distanceToPlayer <= controller.idleDistance)
        {
            controller.SetState(idleState);
        }
            controller.navMeshAgent.SetDestination(controller.player.position);
    }

    public override void ExitState(FSMController controller)
    {
    }

    public override void CheckTransitions(FSMController controller)
    {
        Vector3 playerVelocity = controller.player.GetComponent<Rigidbody>().velocity;

        if (controller.distanceToPlayer <= controller.stopRunning)
        {
            controller.SetState(followState);
        }
    }
}
