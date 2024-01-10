using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerMenu : MonoBehaviour
    {
        [SerializeField] PlayerMenuManager MenuManager;

        public void OnClick_Item()
        {
            MenuManager.OpenMenu(PlayerMenuEnum.Item, gameObject);
        }
        public void OnClick_File()
        {
            MenuManager.OpenMenu(PlayerMenuEnum.File, gameObject);
        }

        public void OnClick_Map()
        {
            MenuManager.OpenMenu(PlayerMenuEnum.Map, gameObject);
        }

        public void OnClick_Exit()
        {
            MenuManager.OpenMenu(PlayerMenuEnum.Exit, gameObject);
        }


    }
}
