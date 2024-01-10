using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class MapMenu : MonoBehaviour
    {
        [SerializeField] PlayerMenuManager MenuManager;

        public void OnClick_Item()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.Item, gameObject);
        }
        public void OnClick_File()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MenuManager.OpenMenu(PlayerMenuEnum.File, gameObject);
        }
    }
}
