using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "FSM/States/FollowState")]
public class FollowStateSO : StateSO
{
    public StateSO idleState;
    public StateSO runState;
    private float nextUpdateTime = 0f;
    private float updateInterval = 0.1f;

    public override void EnterState(FSMController controller)
    {
        controller.characterController.IsSprinting = false;
        controller.characterController.PlayerAnimator.SetBool(FSMController.FIRING, false);
    }

    public override void ExecuteState(FSMController controller)
    {
        //if (controller.distanceToPlayer <= controller.idleDistance)
        //{
        //    controller.SetState(idleState);
        //}
        //else 
        if (Time.time >= nextUpdateTime)
        {
            controller.navMeshAgent.SetDestination(controller.player.position);
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    public override void ExitState(FSMController controller)
    {
    }

    public override void CheckTransitions(FSMController controller)
    {
        Vector3 playerVelocity = controller.player.GetComponent<Rigidbody>().velocity;

        if (controller.distanceToPlayer > controller.startRunning)
        {
            controller.SetState(runState);
            return;
        }

        if (controller.distanceToPlayer <= controller.idleDistance) // Adjust 0.1f as needed
        {
            controller.SetState(idleState);
            return;
        }
        else if(controller.navMeshAgent.velocity.magnitude < 0.1f)
        {
            controller.SetState(idleState);
            return;
        }


    }


}
