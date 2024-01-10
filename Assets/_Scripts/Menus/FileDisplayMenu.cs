using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class FileDisplayMenu : MonoBehaviour
    {
        [SerializeField] PlayerMenuManager MenuManager;
             
        public void OnClick_File()
        {
                SoundManagement.Instance.MenuSound(MenuSounds.Accept);
                MenuManager.OpenMenu(PlayerMenuEnum.File, gameObject);
        }

    }
}
