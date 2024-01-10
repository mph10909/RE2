using System.Collections.Generic;
using UnityEngine;

public class CharacterPositionChecker : MonoBehaviour
{

    [SerializeField] CharacterManager charactermanager; // List of characters to check
    public float rangeThreshold = 30f; // The maximum allowed range

    public bool optionToEnable; // The option to enable if all characters are within range

    private void Update()
    {

        optionToEnable = CheckAllCharactersWithinRange();
    }

    private bool CheckAllCharactersWithinRange()
    {
        foreach (GameObject character in charactermanager.characters)
        {
            float distance = Vector3.Distance(transform.position, character.transform.position);

            // If any character is outside the range, return false
            if (distance > rangeThreshold)
            {
                return false;
            }
        }

        // If all characters are within the range, return true
        return true;
    }
}
