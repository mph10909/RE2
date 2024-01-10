using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;


namespace ResidentEvilClone
{
    public class InventoryToggle : MonoBehaviour
    {
        [SerializeField] GameObject inventoryScreen, storageScreen;

        [SerializeField] AudioMixer audioMixer;
        [SerializeField][Range(0.0001f, 1)] float soundFXVolume;

        public void Inventory(InputAction.CallbackContext context)
        {
            if (storageScreen.activeSelf) return;
            if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
            if (!context.canceled) return;
            Enabler();
        }

        public void Enabler()
        {
            if (!inventoryScreen.activeSelf)
            {
                audioMixer.SetFloat("SoundFx", Mathf.Log10(soundFXVolume) * 20);
                inventoryScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {

                SoundManagement.Instance.MenuSound(MenuSounds.Cancel);
                audioMixer.SetFloat("SoundFx", Mathf.Log10(1) * 20);
                inventoryScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
