using UnityEngine;
using System.Collections;
[CreateAssetMenu(menuName = "FSM/States/ReloadState")]
public class ReloadStateSO : StateSO
{
    public StateSO aimState;

    public override void EnterState(FSMController controller)
    {
        controller.navMeshAgent.SetDestination(controller.transform.position);
    }

    public override void ExecuteState(FSMController controller)
    {

    }

    public override void ExitState(FSMController controller)
    {
    }

    public override void CheckTransitions(FSMController controller)
    {
        if (!controller.characterController.PlayerAnimator.GetBool(FSMController.RELOAD))
        {
            controller.SetState(aimState);
        }
    }
}
