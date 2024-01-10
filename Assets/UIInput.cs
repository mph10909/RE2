using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace ResidentEvilClone
{
    public class UIInput : MonoBehaviour, IUIInput
    {
        private PlayerInputProcessor m_ToggleInventoryP = new PlayerInputProcessor();
        private PlayerInputProcessor m_TogglePauseP = new PlayerInputProcessor();
        private PlayerInputProcessor m_CancelP = new PlayerInputProcessor();


        private void OnPause(InputValue value)
        {
            m_TogglePauseP.Process(value);
        }

        private void OnInventory(InputValue value)
        {
            m_ToggleInventoryP.Process(value);
        }

        private void OnCancel(InputValue value)
        {
            m_CancelP.Process(value);
        }

        public bool IsToggleInventoryDown()
        {
            return m_ToggleInventoryP.IsDown();
        }

        public bool IsCancelDown()
        {
            return m_CancelP.IsDown();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public bool IsTogglePauseDown()
        {
            throw new NotImplementedException();
        }
    }
}
