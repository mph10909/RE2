using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DisplayText : MonoBehaviour
{
    PlayerAction playerActions;
    GameObject textDisplay;
    [HideInInspector] [TextArea(3,5)] public string _displayText = "";
    string[] multiLineText;
    int i;

    public string DisplayTexts { set { textDisplay.GetComponent<Text>().text = value; } }

    public virtual void Awake()
    {
        playerActions = new PlayerAction();
        playerActions.Player.Enable();
        textDisplay = GameObject.FindGameObjectWithTag("TextDisplay");
        //playerActions.Player.ClearText.performed += InputTextClear;
    }

    public void TextDisplay(string displayText)
    {
        DisplayTexts = displayText;
        
        Time.timeScale = 0;
        GameStateManager.Instance.SetState(GameState.Paused);
        Actions.TextClear += TextClear;
        return;
    }

    public void TextDisplayMulti(string[] displayText)
    {
        multiLineText = displayText;
        textDisplay.GetComponent<Text>().text = multiLineText[i];
        Time.timeScale = 0;
        GameStateManager.Instance.SetState(GameState.Paused);
        //Actions.TextClear += NextLine;
        return;
    }

    public void UnclearableTextDisplay(string displayText)
    {
        Actions.TextClear -= TextClear;
        DisplayTexts = displayText;
        GameStateManager.Instance.SetState(GameState.Paused);
        return;
    }

    public void NextLine()
    {
        DisplayTexts = _displayText;
        Actions.TextClear -= NextLine;
        Actions.TextClear += TextClear;
    }

    public void TextClear()
    {
        DisplayTexts = "";
        Time.timeScale = 1;
        GameStateManager.Instance.SetState(GameState.GamePlay);
        Actions.RotationComplete?.Invoke(false);
        Actions.TextClear -= TextClear;
        return;
    }

    public void InputTextClear(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(context.performed)TextClear();
        }
        
    }
}
