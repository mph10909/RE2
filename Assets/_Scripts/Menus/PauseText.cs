using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class PauseText : MonoBehaviour
    {
        RE playerAction;

        [SerializeField] AudioMixer audioMixer;
        [SerializeField] GameObject pauseScreen;
        [SerializeField][Range(0.0001f, 1)] float pauseVolume;
        [SerializeField][Range(0.0001f, 1)] float soundFXVolume;
        [SerializeField] GameObject[] activeMenuCheck;

        void Awake()
        {
            playerAction = new RE();
        }

        public void Pause(InputAction.CallbackContext context)
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
            if (!context.canceled) return;
            if (canEnable)
                {
                    Enabler();
                }

        }

        public float currentSoundFXLevel;

        private void Enabler()
        {
            if (!pauseScreen.activeSelf)
            {
                // Store current soundFX level
                audioMixer.GetFloat("SoundFx", out currentSoundFXLevel);
                pauseScreen.SetActive(true);
                Cursor.visible = false;
                audioMixer.SetFloat("SoundFx", Mathf.Log10(soundFXVolume) * 20);
                audioMixer.SetFloat("Volume", Mathf.Log10(pauseVolume) * 20);
                Time.timeScale = 0;
            }
            else
            {
                audioMixer.SetFloat("Volume", Mathf.Log10(1) * 20);
                audioMixer.SetFloat("SoundFx", currentSoundFXLevel); // set back to stored level
                Cursor.visible = true;
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
            Actions.PauseCharacter?.Invoke(Time.timeScale);
        }
    }
}
