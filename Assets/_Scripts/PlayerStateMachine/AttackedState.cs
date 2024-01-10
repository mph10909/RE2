using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class AttackedState : State
    {
        public AttackedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            throw new NotImplementedException();
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        public override void ExitState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void InitializeSubState()
        {
        }
    }
}
