using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class TutorialPause : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu, InventoryMenu;
        PlayerInput playerInput;
        bool paused, inventory;

        void Start()
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (!paused && !inventory)
                {
                    if (Time.timeScale == 0) return;
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);               
                }
                else
                {
                    Time.timeScale = 1;
                    pauseMenu.SetActive(false);
                }

                paused = !paused;
            }
        }

        public void Inventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!inventory && !paused)
                {
                    if (Time.timeScale == 0) return;
                    Time.timeScale = 0;
                    InventoryMenu.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    InventoryMenu.SetActive(false);
                }

                inventory = !inventory;
            }
        }

    }
}
