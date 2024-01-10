using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class PlayerStateMachine : MonoBehaviour
    {
        State _currentState;
        PlayerStateFactory _states;

        #region SerializedFields

        [Header("Movement Speeds")]
        [SerializeField] float _idleSpeed = 0f;
        [SerializeField] float _walkSpeed = 4f;
        [SerializeField] float _backwardSpeed = -1f;
        [SerializeField] float _sprintSpeed = 10f;
        [SerializeField] float _rotateSpeed = 3.0f;
        [SerializeField] float _standingRotationSpeed = 2f;
        [SerializeField] float _quickTurnRotationTime = .5f;

        [Header("Weapon Draw Speed")][Space(2)]
        [SerializeField] float _weaponDraw = 10f;

        [Header("Falling")][Space(2)]
        [SerializeField] float _gravity = 25f;
        [SerializeField] float _ledgeDropDistance = 3;
        #endregion

        PlayerAction playerActions;
        AnimationController playerAnim;
        CharacterController character;

        float _currentSpeed, _stepOffset, _currentRotateSpeed, _distanceDown;
        bool _reloading, _firePressed, _reloadPressed, _isFalling, _climbing, _quickTurn,  _climbOff, _canMove;
        private Vector3 _direction;
        private int frameCount = 0;

        #region Get Or Set
        public Animator PlayerAnimator { get { return PlayerAnim.Anim; } set { PlayerAnim.Anim = value; } }
        public State CurrentState { get { return _currentState; } set { _currentState = value; } }
        public PlayerStateFactory States { get { return _states; } }
        public PlayerAction PlayerActions { get { return playerActions; } }
        public AnimationController PlayerAnim { get { return playerAnim; } set { playerAnim = value; } }

        public float IdleSpeed { get { return _idleSpeed; }}
        public float WalkSpeed { get { return _walkSpeed; }}
        public float SprintSpeed { get { return _sprintSpeed; }}
        public float BackwardSpeed { get { return _backwardSpeed; } set { _backwardSpeed = value; } }
        public float CurrentSpeed { get { return _currentSpeed; } set { _currentSpeed = value; } }

        public float CurrentRotateSpeed { get { return _currentRotateSpeed; } set { _currentRotateSpeed = value; } }
        public float WeaponDraw { get { return _weaponDraw; } }
        public float RotateSpeed { get { return _rotateSpeed; } }
        public float StandingRotationSpeed { get { return _standingRotationSpeed; } }

        public bool SouthButtonPress { get { return playerActions.Player.Fire.ReadValue<float>() > 0; } }
        public bool Aiming { get { return playerActions.Player.Aiming.ReadValue<float>() > 0; } }
        public bool Sprinting { get { return playerActions.Player.Sprint.ReadValue<float>() > 0; } }
        public bool AimUp { get { return playerActions.Player.AimDirection.ReadValue<float>() > 0; } }
        public bool AimDown { get { return playerActions.Player.AimDirection.ReadValue<float>() < 0; } }
        public bool AimForward { get { return playerActions.Player.AimDirection.ReadValue<float>() == 0; } }
        public bool CanMove { get { return _canMove; } set { _canMove = value; } }

        public Vector2 MoveInput { get { return playerActions.Player.Move.ReadValue<Vector2>(); } }



        #endregion

        #region Set PlayerAction "Enable/Disable"
        void OnEnable()
        {
            playerActions.Player.Enable();
        }

        void OnDisable()
        {
            playerActions.Player.Disable();
        }
        #endregion

        void Awake()
        {
            _currentRotateSpeed = _rotateSpeed;
            playerAnim = GetComponent<AnimationController>();
            playerActions = new PlayerAction();
            character = GetComponent<CharacterController>();

            _states = new PlayerStateFactory(this);
            _currentState = _states._GroundedState();
            _currentState.EnterState();
            //PlayerActions.Player.Fire.performed += Fire; 
        }

        void Update()
        {
            CurrentState.UpdateStates();
            Turning();
            Movement();
                      
        }

        private void Turning()
        {
            if (Mathf.Abs(MoveInput.x) > 0)
            {      
                if (MoveInput.y == 0.0f && MoveInput.x != 0)
                {
                    CurrentRotateSpeed = StandingRotationSpeed;
                        if (Aiming)
                        {
                            playerAnim.UpdateAnimator(0.0f);
                        }
                        else
                        {
                            playerAnim.UpdateAnimator(0.5f);
                        }
                }
                else {CurrentRotateSpeed = RotateSpeed;}
                transform.Rotate(0, MoveInput.x * CurrentRotateSpeed, 0);
            }
        }

        private void Movement()
        {
            if (CanMove)
            {
                Vector3 forward = transform.forward;
                _direction = forward * CurrentSpeed;
                character.Move(_direction * Time.deltaTime);
            }
            else PlayerAnim.UpdateAnimator(0);
        }

        //void Fire(InputAction.CallbackContext context)
        //{
        //    if(context.performed) print("Fire Performed");

        //}
    }
}
