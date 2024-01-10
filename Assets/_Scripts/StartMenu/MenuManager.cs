using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    
    public static class MenuManager
    {
        public static bool IsInitialised{ get; private set; }
        public static GameObject mainMenu, loadMenu, settingsMenu;

        public static void Init()
        {
            GameObject canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
            mainMenu = canvas.transform.Find("MainMenu").gameObject;
            loadMenu = canvas.transform.Find("LoadMenu").gameObject;
            settingsMenu = canvas.transform.Find("ControlMenu").gameObject;
            IsInitialised = true;
            
        }

        public static void OpenMenu(Menu menu, GameObject cullingMenu)
        {
            if (!IsInitialised)
                Init();
            switch (menu)
            {
                case Menu.Main_Menu:
                    mainMenu.SetActive(true);
                    break;
                case Menu.Load:
                    loadMenu.SetActive(true);
                    break;
                case Menu.Settings:
                    settingsMenu.SetActive(true);
                    break;

            }

            cullingMenu.SetActive(false);
        }
    }
}
