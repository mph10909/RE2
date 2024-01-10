using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class IdleState : State
    {
        public IdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {}

        public override void CheckSwitchState()
        {
            if(Context.MoveInput.y > 0 && Context.Sprinting) { SwitchState(Factory._RunState()); }
            else if(Context.MoveInput.y > 0) { SwitchState(Factory._WalkState()); }
            else if(Context.MoveInput.y < 0) { SwitchState(Factory._WalkBackState()); }
        }

        public override void EnterState()
        {

        }

        public override void ExitState()
        {
        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            Context.CurrentSpeed = Context.IdleSpeed;
            Context.PlayerAnim.UpdateAnimator(0.0f);
            CheckSwitchState();
        }


    }
}
