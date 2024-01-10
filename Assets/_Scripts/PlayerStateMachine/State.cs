using UnityEngine;

namespace ResidentEvilClone
{
    public abstract class State
    {
        private bool _isRootState = false;
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private State _currentSubState;
        private State _currentSuperState;

        protected bool IsRootState { set { _isRootState = value; } }
        protected PlayerStateMachine Context { get { return _ctx; } }
        protected PlayerStateFactory Factory { get { return _factory; } }
        protected State SubState { get { return _currentSubState; } }
        protected State SuperState { get { return _currentSuperState; } }

        public State(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }

        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void CheckSwitchState();

        public abstract void ExitState();

        public abstract void InitializeSubState();

        public void UpdateStates()
        {
           
            UpdateState();
            if(_currentSubState != null)
            {
                _currentSubState.UpdateStates();
               
            }
        }

        public void ExitStates()
        {
            ExitState();
            if(_currentSubState != null)
            {
                _currentSubState.ExitState();
            }
        }

        protected void SwitchState(State newState)
        {
            ExitState();

            newState.EnterState();

            if (_isRootState)
            {
                _ctx.CurrentState = newState;
            }
            else if(_currentSuperState != null)
            {
                _currentSuperState.SetSuperState(newState);
                Debug.Log("New Super State = " + newState);
            }

        }

        protected void SetSuperState(State newSuperState) {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(State newSubState) {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}
