using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BackwardsState : State
    {
        public BackwardsState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void CheckSwitchState()
        {
            if (Context.MoveInput.y == 0) { SwitchState(Factory._IdleState()); }
            else if (Context.MoveInput.y > 0 && Context.Sprinting) { SwitchState(Factory._RunState()); }
            else if (Context.MoveInput.y > 0 && !Context.Sprinting) { SwitchState(Factory._WalkState()); }
        }

        public override void EnterState() { }




        public override void ExitState() { }

        public override void InitializeSubState() { }

        public override void UpdateState()
        {
            CheckSwitchState();
            Context.CurrentSpeed = Context.BackwardSpeed;
            Context.PlayerAnim.UpdateAnimator(-0.5f);
        }
    }
}
