using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class ControlMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject selectedButton, loadStart;

        void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }

        public void OnClick_MainMenu()
        {
            StartCoroutine(Main(0.15f));
            EventSystem.current.SetSelectedGameObject(loadStart);
        }

        IEnumerator Main(float time)
        {
            Fader.Instance.FadeOut(time, false);
            yield return new WaitForSeconds(time);
            MenuManager.OpenMenu(Menu.Main_Menu, gameObject);
        }
    }

}
