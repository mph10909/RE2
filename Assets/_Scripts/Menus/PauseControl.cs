using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
            //UIDisabler.DisableUI();
        }
    }

    void PauseGame ()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else 
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}

//if (!PauseControl.gameIsPaused) Command To Use
