using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour
{
    public static TextTyper Instance { get; private set; }

    [SerializeField] Button skipButtonMulti;            // Reference to the skip button
    [SerializeField] Button skipButtonSinge;            // Reference to the skip button
    [SerializeField] Text displayText;                  // Reference to the text component to display the text
    [SerializeField] AudioSource audioSource;           // Reference to the audio source component
    [SerializeField] AudioClip clickSound;              // Reference to the audio clip to play on click
    [SerializeField] [TextArea(5,5)] string[] texts;    // Array of texts to be displayed
    [SerializeField] [Range(0,5)] float speed;          // Speed of text typing
    [SerializeField] [Range(0,5)] int click;            // Number of characters after which click sound is played
    int currentTextIndex = 0;                           // Index of the current text being displayed
    bool textTyping;                                    // Flag to keep track of if text is currently being typed
    int charIndex;                                      // Index of the current character being displayed

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }                                    

    public Text TextArea
        {
            set { displayText = value; }
        }

    // Coroutine to type out the text character by character
    private IEnumerator TypeOutText(string textToType, float typeSpeed, int soundClick)
    {
        textTyping = true;      // Set text typing flag to true
        charIndex = 0;
        int tick = 0;
        displayText.text = "";  // Clear the display text

        // Loop through each character of the text
        while (charIndex < textToType.Length)
        {
            charIndex++;
            tick++;

            string text = textToType.Substring(0, charIndex);
            text += "<color=#00000000>" + textToType.Substring(charIndex) + "</color>";

            // Check if click sound should be played
            if (tick % soundClick == 0)
            {
                if (clickSound != null) audioSource.PlayOneShot(clickSound);
            }

            displayText.text = text;
            yield return new WaitForSeconds(typeSpeed);
        }

        textTyping = false; // Set text typing flag to false
    }

    // Function to display all text
    public void MultiLineText(string[]textToType, float typeSpeed, int soundClick)
    {
        // Check if text is currently being typed
        if (textTyping)
        {
            StopAllCoroutines(); // Stop the text typing coroutine
            displayText.text = textToType[currentTextIndex]; // Display all text
            textTyping = false; // Set text typing flag to false
        }
        else
        {
            currentTextIndex = (currentTextIndex + 1) % texts.Length; // Get the next text index

            // Check if we have reached the end of the texts array
            if (currentTextIndex == 0)
            {
                // Quit the application if running in the Unity editor
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            else
            {
                 StartCoroutine(TypeOutText(textToType[currentTextIndex], typeSpeed, soundClick)); // Start typing the next text
            }
        }
    }

    public void SingleLineText(string textToType, float typeSpeed, int soundClick)
    {
        
        // Check if text is currently being typed
        if (textTyping)
        {
            StopAllCoroutines(); // Stop the text typing coroutine
            displayText.text = textToType; // Display all text
            textTyping = false; // Set text typing flag to false
        }
        else 
        {
            currentTextIndex = 0;
            StartCoroutine(TypeOutText(textToType, typeSpeed, soundClick));
            // Quit the application if running in the Unity editor
            //#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
            //#endif
            //Application.Quit();
        }
    }

    // Start function to set up the button click listener and start typing the first text
    private void Start()
    {
        //skipButtonMulti.onClick.AddListener(() => MultiLineText(texts, speed, click)); // Add listener to skip button click

        //StartCoroutine(TypeOutText(texts[currentTextIndex], speed, click));
    }

}

