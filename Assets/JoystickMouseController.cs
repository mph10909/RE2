using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickMouseController : MonoBehaviour
{
    public float mouseSpeed = 1f;
    public float joystickThreshold = 0.15f;

    public Vector2 previousMousePosition;
    public Vector2 mouseStartPosition;
    public Camera mainCamera;

    void OnEnable()
    {
        Actions.SetCamera += SetCamera;
    }

    void OnDisable()
    {
        Actions.SetCamera += SetCamera;
    }

    private void SetCamera(GameObject camera)
    {
        mainCamera = camera.GetComponent<Camera>();
    }

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("No camera assigned to JoystickMouseController!");
            return;
        }

        mouseStartPosition.x = mainCamera.scaledPixelWidth / 2;
        mouseStartPosition.y = mainCamera.scaledPixelHeight / 2;

        Mouse.current.WarpCursorPosition(mouseStartPosition);
        previousMousePosition = mouseStartPosition;
    }

    private void FixedUpdate()
    {
        if (Gamepad.current == null || Mouse.current == null)
            return;

        Vector2 joystickInput = Gamepad.current.leftStick.ReadValue();
        Vector2 mouseMovements = Mouse.current.delta.ReadValue();
        float mouseMagnitude = mouseMovements.magnitude;

        if (mouseMagnitude > 0.5f)
        {
            previousMousePosition = Mouse.current.position.ReadValue();
        }

        if (joystickInput.magnitude > joystickThreshold)
        {
            // Calculate the mouse movement based on joystick input
            Vector2 mouseMovement = joystickInput * mouseSpeed * Time.fixedDeltaTime;

            // Update the previous mouse position
            previousMousePosition += mouseMovement;

            // Clamp the mouse position within the screen boundaries
            previousMousePosition = ClampMousePosition(previousMousePosition);

            // Set the new mouse position
            Mouse.current.WarpCursorPosition(previousMousePosition);
        }
    }

    private Vector2 ClampMousePosition(Vector2 position)
    {
        // Get the screen dimensions
        Vector2 screenSize = new Vector2(mainCamera.scaledPixelWidth, mainCamera.scaledPixelHeight);

        // Clamp the position within the screen boundaries
        position = Vector2.Max(position, Vector2.zero);
        position = Vector2.Min(position, screenSize);

        return position;
    }
}
