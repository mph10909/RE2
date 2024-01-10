using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPadController : MonoBehaviour
{
    [SerializeField] private List<Text> digits;  // The digits in the number pad
    [SerializeField] private List<int> code; // The correct code
    [SerializeField] private Image[] codeIndicators; // The indicators showing whether the code is correct
    [SerializeField] private Sprite correctCodeSprite; // The sprite to display when the code is correct
    [SerializeField] private Sprite incorrectCodeSprite; // The sprite to display when the code is incorrect

    public int currentRowIndex = 0; // The index of the currently selected row
    public int currentColIndex = 0; // The index of the currently selected column
    private bool isCodeCorrect = false; // Whether the code is currently correct

     int[,] numberLayout = new int[3, 4] { { 6, 4, 1, -1 }, { 8, 5, 2, 0 }, { 9, 6, 3, -1 } }; // The layout of the number pad
    public int nextRowIndex;
    public int nextColIndex;
    public int nextDigitValue;

    private void Start()
    {
        // Set the initial color of the first digit to grey
        digits[currentRowIndex * 4 + currentColIndex].color = Color.grey;
    }

    private void Update()
    {
        // Move the selection up when the up arrow or W key is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveSelection(-1, 0);
        }

        // Move the selection down when the down arrow or S key is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveSelection(1, 0);
        }

        // Move the selection left when the left arrow or A key is pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveSelection(0, -1);
        }

        // Move the selection right when the right arrow or D key is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveSelection(0, 1);
        }

        // Register the selected digit when the enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RegisterDigitSelection();
        }
    }

    private void MoveSelection(int rowDirection, int colDirection)
    {
        // Calculate the index of the next row and column
        nextRowIndex = currentRowIndex + rowDirection;
        nextColIndex = currentColIndex + colDirection;

        // If the next row or column index is out of range, return
        if (nextRowIndex < 0 || nextRowIndex > 2)
        {
            return;
        }

        // If the next column index is greater than 3, wrap around to the first column
        if (nextColIndex > 3)
        {
            nextColIndex = 0;
        }

        // If the next column index is less than 0, wrap around to the last column
        if (nextColIndex < 0)
        {
            nextColIndex = 3;
        }

        // Get the value of the digit at the next row and column indices
        nextDigitValue = numberLayout[nextRowIndex, nextColIndex];

        // If the next digit value is -1, return
        if (nextDigitValue == -1)
        {
            return;
        }

        // Set the color of the current digit back to white
        digits[currentRowIndex * 4 + currentColIndex].color = Color.white;

        // Update the current row and column indices to the next row and column indices
        currentRowIndex = nextRowIndex;
        currentColIndex = nextColIndex;

        // Set the color of the new current digit to grey
        digits[currentRowIndex * 4 + currentColIndex].color = Color.gray;

        // Check if the current code matches the correct code
        isCodeCorrect = CheckCode();

        // Update the code indicators
        UpdateCodeIndicators();
    }


    private bool CheckCode()
    {
        // If the number of selected digits does not match the length of the code, return false
        if (code.Count != digits.Count)
        {
            return false;
        }

        // Check if each selected digit matches the corresponding digit in the code
        for (int i = 0; i < code.Count; i++)
        {
            if (code[i] != digits[i].text[0] - '0')
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateCodeIndicators()
    {
        // Set the sprites of the code indicators based on whether the code is correct
        for (int i = 0; i < codeIndicators.Length; i++)
        {
            codeIndicators[i].sprite = isCodeCorrect ? correctCodeSprite : incorrectCodeSprite;
        }
    }

    private void RegisterDigitSelection()
    {
        // If the number of selected digits is equal to the length of the code, return
        if (code.Count == digits.Count)
        {
            return;
        }

        // Add the value of the currently selected digit to the code
        code.Add(digits[currentRowIndex * 4 + currentColIndex].text[0] - '0');

        // Check if the current code matches the correct code
        isCodeCorrect = CheckCode();

        // Update the code indicators
        UpdateCodeIndicators();
    }
}
