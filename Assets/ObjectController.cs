using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public List<GameObject> gameObjects; // A list of GameObjects you will drag into this field in the Unity Inspector
    private int currentObjectIndex = -1; // Initialized to -1 so the first object becomes the active one when you first press 'E'

    public float moveSpeed = 5.0f; // Speed at which the game objects move

    private void Update()
    {
        RemoveDisabledOrDestroyedObjects();
        HandleSwitchControl();
        HandleMovement();
    }

    private void RemoveDisabledOrDestroyedObjects()
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i] == null || !gameObjects[i].activeInHierarchy)
            {
                gameObjects.RemoveAt(i);
                if (i <= currentObjectIndex)
                {
                    currentObjectIndex--;
                }
            }
        }
    }

    private void HandleSwitchControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Deactivate current object's control (if there's any active)
            if (IsValidIndex(currentObjectIndex))
            {
                DeactivateControl(gameObjects[currentObjectIndex]);
            }

            // Move to the next GameObject
            currentObjectIndex++;
            if (currentObjectIndex >= gameObjects.Count)
            {
                currentObjectIndex = 0; // Loop back to the beginning of the list if we've reached the end
            }

            // Activate the new object's control
            if (IsValidIndex(currentObjectIndex))
            {
                ActivateControl(gameObjects[currentObjectIndex]);
            }
        }
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < gameObjects.Count && gameObjects[index] != null && gameObjects[index].activeInHierarchy;
    }


    private void HandleMovement()
    {
        if (currentObjectIndex >= 0 && currentObjectIndex < gameObjects.Count)
        {
            GameObject activeObject = gameObjects[currentObjectIndex];
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            activeObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void ActivateControl(GameObject obj)
    {
        // Your code to activate control for the GameObject. 
        // For now, it's empty since we're directly using the movement logic in the HandleMovement() method.
    }

    private void DeactivateControl(GameObject obj)
    {
        // Your code to deactivate control for the GameObject. 
        // For now, it's empty since we're directly using the movement logic in the HandleMovement() method.
    }
}
