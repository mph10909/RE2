using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float mouseSpeed = 1f;

    private Vector2 joystickInput;
    private Vector2 previousMousePosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        previousMousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    void Update()
    {
        // Get joystick input from the input system
        joystickInput = Gamepad.current.leftStick.ReadValue();

        // Calculate mouse movement based on joystick input
        Vector2 mouseMovement = joystickInput * mouseSpeed * Time.deltaTime;

        // Get the current mouse position in screen space
        Vector2 currentMousePositionScreen = Mouse.current.position.ReadValue();

        // Convert screen space mouse position to world space
        Vector2 currentMousePositionWorld = mainCamera.ScreenToWorldPoint(currentMousePositionScreen);

        // Calculate the new mouse position based on movement
        Vector2 newMousePositionWorld = currentMousePositionWorld + mouseMovement;

        // Convert the new mouse position back to screen space
        Vector2 newMousePositionScreen = mainCamera.WorldToScreenPoint(newMousePositionWorld);

        // Move the mouse to the new position in screen space
        Mouse.current.WarpCursorPosition(newMousePositionScreen);

        // Remember the current mouse position for the next frame
        previousMousePosition = newMousePositionWorld;
    }
}
