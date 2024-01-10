using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PauseController : MonoBehaviour
    {
        public static PauseController Instance { get; private set; }

        private bool isPaused = false; // Field to track pause status

        // Public property to check if the game is paused
        public bool IsPaused
        {
            get { return isPaused; }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Pause()
        {
            if (!isPaused) // Only pause if not already paused
            {
                Time.timeScale = 0;
                isPaused = true; // Update the status
            }
        }

        public void Resume()
        {
            if (isPaused) // Only resume if currently paused
            {
                Time.timeScale = 1;
                isPaused = false; // Update the status
            }
        }
    }
}
