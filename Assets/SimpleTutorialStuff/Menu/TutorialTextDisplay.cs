// This script handles the display of tutorial text in a Unity game. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class TutorialTextDisplay : MonoBehaviour
{
    // Reference to the PlayerInput component
    PlayerInput playerInput;

    // Reference to the text component for displaying the tutorial text
    Text displaytext;

    // Boolean to track if text is currently displayed
    bool textDisplayed;

    void Start()
    {
        // Get reference to the PlayerInput component
        playerInput = FindObjectOfType<PlayerInput>();

        // Print current action map for debugging purposes
        print(playerInput.currentActionMap);

        // Get reference to the text component
        displaytext = GetComponent<Text>();

        // Subscribe to the TextMessage event in the TutorialActionMediator class
        TutorialActionMediator.TextMessage += TextDisplay;
    }

    // Event handler for the TextMessage event in the TutorialActionMediator class
    void TextDisplay(string textToDisplay)
    {
        // Only display text if it isn't already displayed
        if (!textDisplayed)
        {
            // Set the time scale to 0, effectively pausing the game
            Time.timeScale = 0;

            // Set the text to display the passed in tutorial text
            displaytext.text = textToDisplay;

            // Set textDisplayed to true
            textDisplayed = true;

            // Switch the current action map to the "UI" map
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    // Callback function for the "UI/Cancel" action in the UI action map
    public void TextClear(InputAction.CallbackContext context)
    {
        // Only clear text if text is displayed and the action has been canceled
        if (textDisplayed && context.performed)
        {
            // Set the time scale back to 1, effectively unpausing the game
            Time.timeScale = 1;

            // Clear the text
            displaytext.text = "";

            // Set textDisplayed to false
            textDisplayed = false;

            // Switch the current action map back to the "Player" map
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}

