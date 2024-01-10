using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class InventoryMenu : MonoBehaviour
    {
        [SerializeField] PlayerMenuManager MenuManager;

        public void OnClick_File()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.File, gameObject);
        }

        public void OnClick_Map()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.Map, gameObject);
        }

    }
}
