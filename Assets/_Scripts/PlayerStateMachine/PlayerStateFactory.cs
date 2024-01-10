namespace ResidentEvilClone
{
    public class PlayerStateFactory
    {
        PlayerStateMachine _context;

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;
        }

        public GroundedState _GroundedState() { return new GroundedState(_context, this); }
        public IdleState _IdleState() { return new IdleState(_context, this); }
        public WalkState _WalkState() { return new WalkState(_context, this); }
        public BackwardsState _WalkBackState() { return new BackwardsState(_context, this); }
        public RunState _RunState() { return new RunState(_context, this); }

        public AttackedState _AttackState() { return new AttackedState(_context, this); }

        public AimState _AimState() { return new AimState(_context, this); }

    }
}
