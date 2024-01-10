using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class GroundedState : State
    {
        public GroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) { IsRootState = true; InitializeSubState(); }

        public override void CheckSwitchState()
        {
            if (Context.Aiming) SwitchState(Factory._AimState());
        }

        public override void EnterState(){
            Context.CanMove = true;
        }

        public override void ExitState(){
            Context.CanMove = false;
        }

        public override void InitializeSubState(){
        }

        public override void UpdateState()
        {
            if (Context.MoveInput.y == 0) { SetSubState(Factory._IdleState());}
            else if (Context.MoveInput.y < 0) { SetSubState(Factory._WalkBackState());}
            else if (Context.MoveInput.y > 0 && !Context.Sprinting) { SetSubState(Factory._WalkState());}
            else if (Context.MoveInput.y > 0 && Context.Sprinting) { SetSubState(Factory._RunState());}

            CheckSwitchState();
        }


    }
}
