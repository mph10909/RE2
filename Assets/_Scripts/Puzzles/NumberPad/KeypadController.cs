using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    [Header("Configuration")]
    public KeypadButton currentlySelectedButton;
    public string correctCombo = "1234";
    public Text displayText;

    private string enteredSequence = "";
    private bool verticalPressed = false;
    private bool horizontalPressed = false;
    private Coroutine shakeCoroutine;
    private Color defaultTextColor;

    private void Start()
    {
        InitializeKeypad();
    }

    private void InitializeKeypad()
    {
        HighlightInitialButton();
        StoreDefaultTextColor();
    }

    private void HighlightInitialButton()
    {
        currentlySelectedButton?.HighlightButton();
    }

    private void StoreDefaultTextColor()
    {
        if (displayText)
        {
            defaultTextColor = displayText.color;
        }
    }

    private void Update()
    {
        HandleNavigationInput();
        HandleButtonPress();
    }

    private void HandleNavigationInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (!verticalPressed && Mathf.Abs(vertical) > 0.1f)
        {
            NavigateToButton(vertical > 0 ? currentlySelectedButton.up : currentlySelectedButton.down);
            verticalPressed = true;
        }
        else if (Mathf.Abs(vertical) < 0.1f)
        {
            verticalPressed = false;
        }

        if (!horizontalPressed && Mathf.Abs(horizontal) > 0.1f)
        {
            NavigateToButton(horizontal > 0 ? currentlySelectedButton.right : currentlySelectedButton.left);
            horizontalPressed = true;
        }
        else if (Mathf.Abs(horizontal) < 0.1f)
        {
            horizontalPressed = false;
        }
    }

    private void HandleButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ProcessCurrentButtonAction();
        }
    }

    private void ProcessCurrentButtonAction()
    {
        if (currentlySelectedButton)
        {
            if (currentlySelectedButton.isBackButton)
                RemoveLastNumberFromSequence();
            else if (currentlySelectedButton.isEnterKey)
                ConfirmSequence();
            else
                AddNumberToSequence(currentlySelectedButton.GetButtonValue());
        }
    }

    private void NavigateToButton(KeypadButton nextButton)
    {
        if (nextButton)
        {
            currentlySelectedButton.UnhighlightButton();
            currentlySelectedButton = nextButton;
            currentlySelectedButton.HighlightButton();
        }
    }

    private void RemoveLastNumberFromSequence()
    {
        if (enteredSequence.Length > 0)
        {
            enteredSequence = enteredSequence.Substring(0, enteredSequence.Length - 1);
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (displayText)
        {
            displayText.text = enteredSequence;
        }
    }

    private void AddNumberToSequence(string number)
    {
        enteredSequence += number;
        UpdateDisplay();
    }

    private void ConfirmSequence()
    {
        if (string.IsNullOrEmpty(enteredSequence))
            return;

        Debug.Log($"Entered Sequence: {enteredSequence}");
        Color resultColor = enteredSequence == correctCombo ? Color.green : Color.red;
        Debug.Log(enteredSequence == correctCombo ? "Correct combo entered!" : "Incorrect combo. Try again.");

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeAndFadeReset(() =>
        {
            enteredSequence = "";
            UpdateDisplay();
        }, resultColor));
    }

    private IEnumerator ShakeAndFadeReset(Action onComplete, Color color)
    {
        yield return ShakeDisplayEffect();
        yield return FadeDisplayEffect(color);
        ResetDisplayColor();

        onComplete?.Invoke();
    }

    private IEnumerator ShakeDisplayEffect()
    {
        Vector3 originalPosition = displayText.transform.localPosition;
        float shakeAmount = 0.5f;
        float shakeDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = originalPosition.x + UnityEngine.Random.Range(-shakeAmount, shakeAmount);
            displayText.transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayText.transform.localPosition = originalPosition;
    }

    private IEnumerator FadeDisplayEffect(Color color)
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        displayText.color = color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetDisplayColor()
    {
        displayText.color = defaultTextColor;
    }
}
