using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject selectedStart;
        [SerializeField] bool gameStarted;

        void OnEnable()
        {
            if (!gameStarted)
            {
                EventSystem.current.SetSelectedGameObject(selectedStart);
                gameStarted = true;
            }
            
        }

        public void OnClick_Start()
        {

        }
        public void OnClick_Load()
        {
            StartCoroutine(MenuSelect(Menu.Load,0.15f));
        }

        public void OnClick_Control()
        {
            StartCoroutine(MenuSelect(Menu.Settings, 0.15f));
        }

        IEnumerator MenuSelect(Menu menu,float time)
        {
            Fader.Instance.FadeOut(time, false);
            yield return new WaitForSeconds(time);
            MenuManager.OpenMenu(menu, gameObject);
        }
    }
}
