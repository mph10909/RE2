using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class MenuDisabled : MonoBehaviour
    {
        [SerializeField] GameObject Item_Menu, File_Menu, Map_Menu;       
        [SerializeField] Button[] Exit_Button;

        void OnDisable()
        {
            Item_Menu.SetActive(true);
            File_Menu.SetActive(false);
            Map_Menu.SetActive(false);
            foreach(Button button in Exit_Button)
            {
                    button.Select();    
            }
        }
    }
}
