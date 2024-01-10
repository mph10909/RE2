using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class WalkState : State
    {
        public WalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void CheckSwitchState()
        {
            if (Context.MoveInput.y == 0) { SwitchState(Factory._IdleState()); }
            else if (Context.MoveInput.y > 0 && Context.Sprinting) { SwitchState(Factory._RunState()); }
            else if (Context.MoveInput.y < 0) { SwitchState(Factory._WalkBackState()); }
        }

        public override void EnterState(){}

        public override void ExitState() { }

        public override void InitializeSubState() { }

        public override void UpdateState()
        {
            Context.CurrentSpeed = Context.WalkSpeed;
            Context.PlayerAnim.UpdateAnimator(0.5f);
            CheckSwitchState();
        }
    }
}
