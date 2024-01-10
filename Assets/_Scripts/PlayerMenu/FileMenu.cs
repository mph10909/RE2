using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class FileMenu : MonoBehaviour
    {
        [SerializeField] PlayerMenuManager MenuManager;
        
        public void OnClick_Item()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.Item, gameObject);
        }

        public void OnClick_Map()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.Map, gameObject);
        }

        public void OnClick_FileDisplay(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            FileDisplay();
        }

        public void OnClick_FileDisplayButton()
        {
            FileDisplay();
        }

        private void FileDisplay()
        {
            if (!this.gameObject.activeSelf) return;
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.FileDisplay, gameObject);
        }
    }
}
