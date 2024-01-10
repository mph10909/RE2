using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RunState : State
    {
        public RunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            if (Context.MoveInput.y == 0) { SwitchState(Factory._IdleState()); }
            else if (Context.MoveInput.y > 0 && !Context.Sprinting) { SwitchState(Factory._WalkState()); }
            else if (Context.MoveInput.y < 0) { SwitchState(Factory._WalkBackState()); }

        }

        public override void EnterState()
        {

        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            CheckSwitchState();
            Context.CurrentSpeed = Context.SprintSpeed;
            Context.PlayerAnim.UpdateAnimator(1f);
        }

        public override void InitializeSubState() { }



    }
}
