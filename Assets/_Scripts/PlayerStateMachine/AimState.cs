using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class AimState : State
    {
        float newValue;

        public AimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (!Context.Aiming && newValue <= 0.01f) { newValue = 0; SwitchState(Factory._GroundedState()); }
        }

        public override void EnterState()
        {
            Context.PlayerAnim.UpdateAnimator(0.0f);
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (Context.PlayerAnim.Firing) return;
            AimingDirection();
            AimWeapon();
            Fire();
            CheckSwitchState();
        }

        private void AimingDirection()
        {

            if (!Context.AimDown && !Context.AimUp) { AimForward(); }
            else if (Context.AimUp) { Context.PlayerAnim.AimDirection = 0.5f; }
            else if (Context.AimDown) { Context.PlayerAnim.AimDirection = -0.5f; }
        }

        public override void InitializeSubState()
        {
        }

        private void AimWeapon()
        {
            float targetValue = Context.Aiming ? 1f : 0f;
            newValue = Mathf.Lerp(Context.PlayerAnim.AimingWeight, targetValue, Time.deltaTime * Context.WeaponDraw);
            newValue = Mathf.Clamp01(newValue);

            Context.PlayerAnim.UpdateAiming(newValue);
        }


        private void Fire()
        {
            if (Context.PlayerAnim.AimingWeight < 0.9f) return;
            if (Context.SouthButtonPress)
            {
                Context.PlayerAnim.UpdateFiring();
                //Actions.FiredWeapon?.Invoke();
            }
        }

        private void AimForward()
        {
            if (!Context.AimDown && !Context.AimUp)
            {
                Context.PlayerAnim.AimDirection = 0.0f;
                if (Mathf.Abs(Context.PlayerAnim.AimDirection) < 0.01f)
                {
                    Context.PlayerAnimator.SetFloat("AimDirection", 0);
                }
            }
        }

    }
}
