using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class PlayerInputProcessor : MonoBehaviour
    {

        public int FrameDown;
        public int FrameUp;

        public bool IsDown() { return FrameDown == Time.frameCount; }
        public bool IsUp() { return FrameUp == Time.frameCount; }
        public bool IsHeld() { return FrameDown != -1 && FrameUp == -1; }

        public void Process(InputValue value)
        {
            if (IsHeld() && !value.isPressed)
            {
                FrameDown = -1;
                FrameUp = Time.frameCount;
            }
            else if (value.isPressed)
            {
                FrameUp = -1;
                FrameDown = Time.frameCount;
            }
        }

        public void Clear()
        {
            FrameDown = -1;
            FrameUp = -1;
        }
    }

    public class PlayerInputListener : MonoBehaviour, IPlayerInput
    {
        private Vector2 m_InputAxis;
        private float m_WeaponAim;
        private PlayerInputProcessor m_AimingP =  new PlayerInputProcessor();
        private PlayerInputProcessor m_AttackP =  new PlayerInputProcessor();
        private PlayerInputProcessor m_RunP =     new PlayerInputProcessor();
        private PlayerInputProcessor m_Turn180P = new PlayerInputProcessor();

        public Vector2 GetPrimaryAxis()
        {
            return m_InputAxis;
        }

        public float WeaponDirection()
        {
            return m_WeaponAim;
        }

        public bool IsWeaponUp()
        {
            throw new NotImplementedException();
        }

        public bool IsWeaponDown()
        {
            throw new NotImplementedException();
        }

        public bool IsAimingHeld()
        {
            return m_AimingP.IsHeld();
        }

        public bool IsAttackDown()
        {
            return m_AttackP.IsDown();
        }

        public bool IsAttackUp()
        {
            return m_AttackP.IsUp();
        }

        public bool IsRunHeld()
        {
            return m_RunP.IsHeld();
        }

        private void OnLook(InputValue value)
        {
            m_InputAxis = value.Get<Vector2>();
        }

        private void OnAiming(InputValue value)
        {
            m_AimingP.Process(value);
        }

        private void OnFire(InputValue value)
        {
            m_AttackP.Process(value);
        }

        public void OnRun(InputValue value)
        {
            m_RunP.Process(value);
        }

        private void OnAimDirection(InputValue value)
        {
            m_WeaponAim = value.Get<float>();
        }




        public void Flush()
        {
            m_AimingP.Clear();
            m_AttackP.Clear();
            m_RunP.Clear();
            m_Turn180P.Clear();
        }


    }
}
