using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class MenuToggle : MonoBehaviour
    {
        [SerializeField] GameObject toggleMenu;
        [SerializeField] GameObject[] activeMenuCheck;

        [SerializeField] AudioMixer audioMixer;
        //[SerializeField][Range(0.0001f, 1)] float soundFXVolume;
        //[SerializeField][Range(0.0001f, 1)] float adjustedSFX;

        // Start is called before the first frame update

        public void MenuToggler(InputAction.CallbackContext context)
        {
            bool canEnable = true;
            foreach (GameObject menu in activeMenuCheck)
            {
                if (menu.activeSelf)
                {
                    canEnable = false;
                    return;
                }
            }
            if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
            if (!context.canceled) return;
            if (canEnable)
            {
                Enabler();
            }
        }


        public void Enabler()
        {
            if (!toggleMenu.activeSelf)
            {
                audioMixer.SetFloat("Enemies", Mathf.Log10(0.0001f) * 20);
                toggleMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {

                SoundManagement.Instance.MenuSound(MenuSounds.Cancel);
                audioMixer.SetFloat("Enemies", Mathf.Log10(1f) * 20);
                toggleMenu.SetActive(false);
                Time.timeScale = 1;
            }
            Actions.PauseCharacter?.Invoke(Time.timeScale);
        }
    }
}
