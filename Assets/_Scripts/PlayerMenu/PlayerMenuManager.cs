using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerMenuManager : MonoBehaviour
    {
        public GameObject itemMenu,  fileMenu, mapMenu, fileDisplayMenu;
        public bool IsInitialised { get; private set; }

        public void Init()
        {
            IsInitialised = true;

        }

        public void OpenMenu(PlayerMenuEnum menu, GameObject cullingMenu)
        {
            if (!IsInitialised)
                Init();
            switch (menu)
            {
                case PlayerMenuEnum.Item:
                    itemMenu.SetActive(true);
                    break;
                case PlayerMenuEnum.File:
                    fileMenu.SetActive(true);
                    break;
                case PlayerMenuEnum.Map:
                    mapMenu.SetActive(true);
                    break;
                case PlayerMenuEnum.FileDisplay:
                    fileDisplayMenu.SetActive(true);
                    break;

            }
            cullingMenu.SetActive(false);
        }
    }
}
