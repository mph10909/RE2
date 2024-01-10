using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class Exit : MonoBehaviour
    {
        [SerializeField]GameObject storageMenu;

        public void OnClick_ExitStorage()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Cancel);
            storageMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
