using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public string[] sentences; // array of strings to hold the dialogue sentences
    public Text dialogueText;  // reference to the UI Text component to display the dialogue

    private int index;         // index of the current sentence being displayed
    private string currentText;// current text being displayed in the UI

    void Start()
    {
        currentText = sentences[index];
        dialogueText.text = currentText;
    }

    void Update()
    {
        // display the next sentence when the player presses the "Next" button
        if (Input.GetButtonDown("Next"))
        {
            index++;

            // if all sentences have been displayed, reset the index to 0
            if (index >= sentences.Length)
            {
                index = 0;
            }

            // update the current text and display it in the UI
            currentText = sentences[index];
            dialogueText.text = currentText;
        }
    }
}

