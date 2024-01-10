using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "FSM/States/FireState")]
public class FireStateSO : StateSO
{

    public StateSO followState;
    public StateSO reloadState;
    public StateSO aimState;
    public StateSO fireState;

    public float aimStateOff = 0.5f;
    public float fireRate = 1.0f;
    private float nextFireTime = 0;

    public override void EnterState(FSMController controller)
    {
        FireWeapon(controller);
    }

    public override void ExecuteState(FSMController controller)
    {

    }

    public override void ExitState(FSMController controller)
    {
        controller.CleanUpEnemiesInRangeList();
    }

    public override void CheckTransitions(FSMController controller)
    {
        if (controller.characterController.PlayerAnimator.GetBool(FSMController.FIRING))
        {
            controller.SetState(aimState);
        }
        if (controller.characterController.PlayerAnimator.GetBool(FSMController.RELOAD))
        {
            controller.SetState(reloadState);
        }
    }

    private void FireWeapon(FSMController controller)
    {
        if (controller.currentState != fireState) return;
        if (!controller.currentTarget) return;

        foreach (Transform child in controller.weaponSlot)
        {
            var fireableWeapon = child.GetComponent<IFireable>();
            if (fireableWeapon != null)
            {

                if (fireableWeapon.AIFire())
                {
                    controller.characterController.PlayerAnimator.SetTrigger(FSMController.FIRE);
                    break;
                }
                else if (controller.characterController.PlayerAnimator.GetBool(FSMController.RELOAD))
                {
                    controller.SetState(reloadState);
                    break;
                }
                else if (!controller.characterController.PlayerAnimator.GetBool(FSMController.AIMING))
                {
                    controller.StateDelay(aimState, aimStateOff);// Might Need To Be Follow
                }
            }
        }
    }

    private bool CanSeeTarget(Transform target, FSMController controller)
    {
        if (target == null) return false;

        RaycastHit hit;
        Vector3 directionToTarget = target.position - controller.transform.position;
        Vector3 rayOrigin = controller.transform.position + Vector3.up * 1.5f; // adjust 1.5f as necessary

        // Draw a ray for visualization in the editor
        Debug.DrawRay(rayOrigin, directionToTarget, Color.red, 1f); // Ray will be red and last for 1 second

        if (Physics.Raycast(rayOrigin, directionToTarget, out hit))
        {
            return hit.transform == target;
        }

        return false;
    }
}
