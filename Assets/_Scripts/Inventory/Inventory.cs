using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool MenuSeletionUp = false;
    public static bool gameIsPaused;
    public static bool gamePauser = false;
    [SerializeField] GameObject MenuCamera, PSXCamera, PlayerCamera, MainMenu;
    [SerializeField] AudioSource MenuNoise;
    [SerializeField] AudioClip[] MenuClick;
    [SerializeField] bool PauseChecker, menuSelectionUp;

    void Awake()
    {
        MenuNoise = GetComponent<AudioSource>();
        MenuNoise.ignoreListenerPause = true;
    }
    void Update()
    {
        menuSelectionUp = MenuSeletionUp;
        PauseChecker = gameIsPaused;
        if(gamePauser) return;
        if(ComponentController.ItemPickUpActive) return;// May need to be changed for item pickup script name
        if(Input.GetButtonDown("Start"))
        {
        if(MenuController.FileMenuActive == true) return;
        if(MenuController.MapMenuActive == true) return;
        if(MenuSeletionUp)MenuOff();
        else MenuOn();
        gameIsPaused = !gameIsPaused; PauseGame();
        }
    }

    void MenuOn()
    {
        
            MenuNoise.PlayOneShot(MenuClick[0]);
            PlayerCamera.SetActive(false);
            PSXCamera.SetActive(false);
            MenuCamera.SetActive(true);
            MainMenu.SetActive(true);
            MenuSeletionUp = true;

    }
    void MenuOff()
    {
            MenuCamera.SetActive(false);
            PlayerCamera.SetActive(true);
            PSXCamera.SetActive(true);
            MainMenu.SetActive(false);
            MenuSeletionUp = false;
    }

    
    public static void PauseGame()
    {
        if(gameIsPaused)
        {
            //MouseLook.mousSensitivity = 0f; 
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else 
        {
            Time.timeScale = 1;
            //MouseLook.mousSensitivity = 100f;
            AudioListener.pause = false;
        }
    }

    public static void PAUSE()
    {
        gamePauser = true;
        gameIsPaused = true;
        PauseGame();
    }

    public static void UNPAUSE()
    {
        gamePauser = false;
        gameIsPaused = false;
        PauseGame();
    }
}
