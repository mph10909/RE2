using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ResidentEvilClone
{
    public class UITextComplete : BaseMessage { }

    public class UIText : MonoBehaviour
    {
        public static UIText Instance { get; private set; }

        [SerializeField] private Text uiText;
        [SerializeField] private GameObject textPanel;

        [Header("Typewriter Sound Settings")]
        [SerializeField] AudioClip typingSound;  
        [SerializeField] private float minPitch = 0.9f;
        [SerializeField] private float maxPitch = 1.1f;
        [SerializeField] private int charactersPerClick = 1;
      

        private Text_Data currentTextData;
        private int currentLineIndex = 0;
        private bool isTextBeingDisplayed = false;
        private bool isTextFullyDisplayed = false;
        private bool requestTextDisplay = false;
        private bool isTyping;

        IPlayerInput m_PlayerInput;

        private void Awake()
        {
            SingletonSetup();
            this.gameObject.SetActive(false);
        }

        private void SingletonSetup()
        {
            if (Instance == null)
            {
                Instance = this;
                m_PlayerInput = GetComponentInParent<IPlayerInput>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartDisplayingText(Text_Data textData, bool typing)
        {
            PauseController.Instance.Pause();
            isTyping = typing;
            gameObject.SetActive(true);

            if (isTextBeingDisplayed)
            {
                ShowFullLineImmediately();
            }
            else
            {
                currentTextData = textData;
                currentLineIndex = 0;
                StartCoroutine(TypeOutText(currentTextData.lines[currentLineIndex]));
            }

            requestTextDisplay = false; // Ensure this flag is reset if this method is called directly.
        }


        public void StartDisplayingText(string text, bool typing)
        {
            isTyping = typing;
            gameObject.SetActive(true);
            

            if (isTextBeingDisplayed)
            {
                ShowFullLineImmediately();
                return;
            }

            Text_Data tempTextData = ScriptableObject.CreateInstance<Text_Data>();
            tempTextData.lines = new string[] { text };
            tempTextData.typingSpeed = 0.05f;
            tempTextData.typingSound = null;

            StartDisplayingText(tempTextData, typing);
        }

        public void StartDisplayingText(string[] text, bool typing)
        {
            isTyping = typing;
            gameObject.SetActive(true);

            if (isTextBeingDisplayed)
            {
                ShowFullLineImmediately();
                return;
            }

            Text_Data tempTextData = ScriptableObject.CreateInstance<Text_Data>();
            tempTextData.lines = text;
            tempTextData.typingSpeed = 0.05f;
            tempTextData.typingSound = null;

            StartDisplayingText(tempTextData, typing);
        }

        private bool IsNewTextData(Text_Data textData)
        {
            return currentTextData != textData;
        }

        private void FinishTextDisplay()
        {
            HideText();
            currentLineIndex = 0;
        }

        private IEnumerator TypeOutText(string line)
        {
            if (!isTyping)
            {
                ShowFullLineImmediately();
                yield break;
            }

            isTextBeingDisplayed = true;
            uiText.text = "";
            int characterCount = 0;
            bool isRichTextContent = false;
            List<string> tagParts = new List<string> { "", "", "", "" };

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != '<')
                {
                    if (isRichTextContent)
                    {
                        tagParts[0] += line[i];
                    }
                    else
                    {
                        uiText.text += line[i];
                        tagParts[3] = uiText.text;
                        characterCount++;

                        if (characterCount >= charactersPerClick)
                        {
                            characterCount = 0;
                            PlayTypingSound();
                        }

                        yield return new WaitForSecondsRealtime(currentTextData.typingSpeed);
                    }
                    continue;
                }

                if (!isRichTextContent)
                {
                    tagParts[1] = GetCompleteRichTextTag(ref i, line);
                    if (!tagParts[1].Contains("="))
                    {
                        uiText.text += tagParts[1];
                        continue;
                    }

                    isRichTextContent = true;
                    continue;
                }

                tagParts[2] = GetCompleteRichTextTag(ref i, line);
                for (int j = 0; j < tagParts[0].Length; j++)
                {
                    uiText.text = tagParts[3] + tagParts[1] + tagParts[0].Substring(0, j + 1) + tagParts[2];
                    characterCount++;

                    if (characterCount >= charactersPerClick)
                    {
                        characterCount = 0;
                        PlayTypingSound();
                    }

                    yield return new WaitForSecondsRealtime(currentTextData.typingSpeed);
                }

                tagParts[3] = uiText.text;
                for (int j = 0; j < tagParts.Count - 1; j++)
                {
                    tagParts[j] = "";
                }
                isRichTextContent = false;
            }

            CompleteLineDisplay();
        }

        string GetCompleteRichTextTag(ref int index, string line)
        {
            string completeTag = string.Empty;

            while (index < line.Length)
            {
                completeTag += line[index];

                if (line[index] == '>')
                    return completeTag;
                index++;
            }
            return string.Empty;
        }

        private void CompleteLineDisplay()
        {
            isTextBeingDisplayed = false;
            isTextFullyDisplayed = currentLineIndex >= currentTextData.lines.Length - 1;
            currentLineIndex++;
        }

        private void PlayTypingSound()
        {
            if (currentTextData.typingSound)
            {
                SoundManagement.Instance.PitchAdjust(Random.Range(minPitch, maxPitch));
                SoundManagement.Instance.PlaySound(currentTextData.typingSound);
            }
            else
            {
                SoundManagement.Instance.PitchAdjust(Random.Range(minPitch, maxPitch));
                SoundManagement.Instance.PlaySound(typingSound);
            }
        }

        public void HideText()
        {
            SoundManagement.Instance.PitchAdjust(1f);
            StopAllCoroutines();
            uiText.text = "";
            PauseController.Instance.Resume();

            isTyping = false;
            ResetTextDisplayFlags();
            textPanel.SetActive(false);
            MessageBuffer<UITextComplete>.Dispatch();

        }

        private void ResetTextDisplayFlags()
        {
            isTextBeingDisplayed = false;
            isTextFullyDisplayed = false;
        }

        public void ShowFullLineImmediately()
        {
            StopAllCoroutines();
            if (currentTextData != null && currentLineIndex < currentTextData.lines.Length)
            {
                uiText.text = currentTextData.lines[currentLineIndex];
            }
            CompleteLineDisplay();
        }

        private void Update()
        {
            if (m_PlayerInput.IsAttackDown())
            {
                if (isTextBeingDisplayed)
                {
                    // If text is currently being displayed, skip to the end of the current text.
                    ShowFullLineImmediately();
                }
                else if (!isTextBeingDisplayed && requestTextDisplay)
                {
                    // If the current text is fully displayed and a new text request is pending, display the new text.
                    StartDisplayingText(currentTextData, isTyping);
                }
                else if (isTextFullyDisplayed)
                {
                    // If the current text is fully displayed and there's no more text to display, hide the text panel.
                    HideText();
                }
                else if (currentLineIndex < currentTextData.lines.Length)
                {
                    // If the current text is fully displayed, start displaying the next line.
                    StartCoroutine(TypeOutText(currentTextData.lines[currentLineIndex]));
                }
            }
        }


    }
}
