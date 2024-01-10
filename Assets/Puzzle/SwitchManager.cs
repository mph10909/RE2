using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwitchManager : DisplayText, IInteractable
{
    RE playerAction;
    [SerializeField] PlayerInput playerInput;

    [SerializeField] AudioClip switchFlip, puzzleSolved, selectSwitch;
    AudioSource audioSource;

    [SerializeField] string puzzleCompletedText;

    public string switchPattern = "00000";
    public int selectedSwitchIndex = 0;

    [SerializeField] GameObject puzzleCamera;

    public NeedleScript needleScript;
    public SwitchScript[] switchScripts;

    private bool inputReleased = true;
    private bool puzzleCompleted;
    private bool puzzleActive;

    private void Start()
    {
        playerAction = new RE();
        audioSource = GetComponent<AudioSource>();
        switchScripts[selectedSwitchIndex].SetSelected(true);
    }

    private void Update()
    {
        if (puzzleCompleted || !puzzleActive) return;

        HandleSwitchSelectionInput();
        HandleSwitchToggleInput();
        HandlePuzzleQuitInput();
    }

    private void HandleSwitchSelectionInput()
    {
        float horizontalInput = playerAction.Interactable_Items.Movement.ReadValue<float>();

        if (horizontalInput != 0 && inputReleased && !needleScript.isNeedleMoving)
        {
            audioSource.PlayOneShot(selectSwitch);
            int prevSelectedSwitchIndex = selectedSwitchIndex;
            selectedSwitchIndex += (int)Mathf.Sign(horizontalInput);

            // Wrap around if index goes out of bounds
            if (selectedSwitchIndex < 0)
            {
                selectedSwitchIndex = switchScripts.Length - 1;
            }
            else if (selectedSwitchIndex >= switchScripts.Length)
            {
                selectedSwitchIndex = 0;
            }

            // Highlight the newly selected switch
            switchScripts[prevSelectedSwitchIndex].SetSelected(false);
            switchScripts[selectedSwitchIndex].SetSelected(true);
            inputReleased = false;
        }
        else if (horizontalInput == 0)
        {
            inputReleased = true;
        }
    }

    private void HandleSwitchToggleInput()
    {
        if (playerAction.Interactable_Items.Select.triggered && !needleScript.isNeedleMoving)
        {
            audioSource.PlayOneShot(switchFlip);
            switchScripts[selectedSwitchIndex].ToggleSwitchState();
        }
    }

    private void HandlePuzzleQuitInput()
    {
        if (playerAction.Interactable_Items.Quit.triggered && !needleScript.isNeedleMoving)
        {
            puzzleActive = false;
            PuzzleBreak();
        }
    }

    public void CheckSwitchPattern()
    {
        bool correctPattern = true;

        for (int i = 0; i < switchPattern.Length; i++)
        {
            if ((switchPattern[i] == '1' && !switchScripts[i].switchState) ||
                (switchPattern[i] == '0' && switchScripts[i].switchState))
            {
                correctPattern = false;
                break;
            }
        }

        if (correctPattern)
        {
            puzzleCompleted = true;
            audioSource.PlayOneShot(puzzleSolved);
            foreach (SwitchScript switches in switchScripts)
            {
                switches.PuzzleFinished();
            }
            PuzzleBreak();
        }
    }

    private void PuzzleBreak()
    {
        playerAction.Player.Enable();
        playerAction.Interactable_Items.Disable();
        Time.timeScale = 1;
        Cursor.visible = true;
        GameStateManager.Instance.SetState(GameState.GamePlay);
        playerInput.SwitchCurrentActionMap("Player");
        puzzleCamera.SetActive(false);
    }

    public void Interact()
    {
        if (!puzzleCompleted)
        {
            playerAction.Player.Disable();
            playerAction.Interactable_Items.Enable();
            playerInput.SwitchCurrentActionMap("Interactable_Items");
            Cursor.visible = false;
            puzzleActive = true;
            puzzleCamera.SetActive(true);
            Time.timeScale = 0;
            GameStateManager.Instance.SetState(GameState.Paused);
        }
        else
        {
            TextDisplay(puzzleCompletedText);
        }

    }
}
