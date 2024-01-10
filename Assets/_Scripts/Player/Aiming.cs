using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class Aiming : MonoBehaviour, IControllable, IComponentSavable
    {
        
        [SerializeField] Animator anim;       
        [SerializeField] PointAndClick _pointAndClick;
        public CharacterData characterData;

        [SerializeField] float turnSpeed;
        [SerializeField] bool setInfoOnAwake;
        public Weapon weaponEquipped;

        int weapon;

        const string FIRE = "Fire";
        const string AIMER = "Aimer", AIMING = "Aiming";
        const string ATTACKED = "Attack";
        const string RELOAD = "Reload";

        public PointAndClick CharacterControl { get { return _pointAndClick; } }

        [SerializeField] private bool isController;

        public float AimDirection { get { return CharacterControl.PlayerAnimator.GetFloat("Aim"); }
                                    set { CharacterControl.PlayerAnimator.SetFloat("Aim", value, 0.1f, Time.deltaTime); } }

        public float RotateCharacterController => Mathf.Clamp(CharacterControl.PlayerActions.Player.Look.ReadValue<Vector2>().x, -1, 1);
        public float RotateCharacterMouse => Mathf.Clamp(CharacterControl.PlayerActions.Player.Look.ReadValue<Vector2>().x, -350, 350);

        public bool AimingUp { get { return m_Input.WeaponDirection() > 0; } }
        public bool AimingDown { get { return m_Input.WeaponDirection() < 0; } }

        public bool CurrentCharacter
        {
            set
            {
                if (CharacterControl != null && CharacterControl.PlayerAnimator != null)
                {
                    CharacterControl.PlayerAnimator.SetBool("CurrentChar", value);
                }
            }
        }

        public float ControllerTurnSpeed { get { return turnSpeed * 100; } }
        public float MouseTurnSpeed { get { return turnSpeed; } }

        public static Dictionary<CharacterData, Aiming> instances = new Dictionary<CharacterData, Aiming>();

        IPlayerInput m_Input;

        void Awake()
        {
            if (!instances.ContainsKey(characterData))
            {
                instances[characterData] = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instances[characterData] != this)
            {
                Destroy(gameObject);
                return;
            }

            m_Input = GetComponentInParent<IPlayerInput>();
            _pointAndClick = GetComponent<PointAndClick>();
            weapon = Animator.StringToHash("Weapon");
            if (setInfoOnAwake) RegisterCallbacks();
        }

        private void OnEnable()
        {
            if(!setInfoOnAwake) RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            CurrentCharacter = true;
            //CharacterControl.PlayerActions.Player.Fire.performed += Fire;
            //print("Fire Register");

        }

        void OnDisable()
        {
            CurrentCharacter = false;
            anim.ResetTrigger("ReloadTrigger");
            anim.ResetTrigger("Fire");
        }

        void Update()
        {
            if (Time.timeScale == 0) return;
            if (weapon == 0) return;
            if (m_Input == null) return;

            if (m_Input.IsAimingHeld())
            {
                EnterAimingState();
            }
            else
            {
                ExitAimingState();
            }

            if(m_Input.IsAimingHeld() && m_Input.IsAttackDown())
            {
                Fire();
            }
        }

        private void EnterAimingState()
        {
            Cursor.visible = false;
            anim.SetBool("Aimer", true);

            SetAimingDirection();
            RotateCharacter();
        }

        private void ExitAimingState()
        {
            Cursor.visible = true;
            anim.SetBool("Aiming", false);
            anim.SetBool("Aimer", false);
        }

        private void SetAimingDirection()
        {

            if (!AimingDown && !AimingUp) {
                AimDirection = 0f;
                if (AimDirection < 0.01f && AimDirection > -0.01f) CharacterControl.PlayerAnimator.SetFloat("Aim", 0);
            }
            else if (AimingUp) AimDirection = 1f;
            else if (AimingDown) AimDirection = -1f;
        }

        void RotateCharacter()
        {
            float speed;

            if (isController) { speed = RotateCharacterController * ControllerTurnSpeed * Time.deltaTime; }
            else { speed = RotateCharacterMouse * Time.deltaTime * MouseTurnSpeed; }
            
            transform.Rotate(0, -speed, 0, Space.Self);
        }

        private void Fire()
        {           
            if (anim.GetBool(RELOAD)) return;
            if (anim.GetBool(ATTACKED)) return;
            if (!anim.GetBool(AIMING)) return;

            weaponEquipped.Firing();
            //Actions.FiredWeapon?.Invoke();
            //print("Fired");
        }

        public void EnableControl(bool enable)
        {
            this.enabled = enable;
        }

        public string GetSavableData()
        {
            return this.enabled.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            if (bool.TryParse(savedData, out bool result))
            {
                this.enabled = result;
            }
        }
    }
}
