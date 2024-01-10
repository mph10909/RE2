using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace ResidentEvilClone
{
    public class GamePadMouse : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;

        [SerializeField] RectTransform canvasRectTransform;

        [SerializeField] RectTransform cursorTransform;
        [SerializeField] float cursorSpeed = 1000;

        [SerializeField] Canvas canvas;

        bool previousMouseState;
        Mouse virtualMouse;
        [SerializeField]Camera mainCamera;

        void OnEnable()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device is Mouse && device.name == "VirtualMouse")
                {
                    Debug.Log("Virtual Mouse is connected to the input system.");
                }
            }
            //VirtualMouse virtualMouse = InputSystem.TryGetDevice<VirtualMouse>();
            if (virtualMouse != null)
            {
                // virtual mouse is added
            }

            if (virtualMouse == null)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }

            InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            if(cursorTransform != null)
            {
                Vector2 position = cursorTransform.anchoredPosition;
                InputState.Change(virtualMouse.position, position);
            }


            InputSystem.onAfterUpdate += UpdateMotion;
            //playerInput.onControlsChanged += OnCon
        }

        void OnDisable()
        {
            InputSystem.RemoveDevice(virtualMouse);
            InputSystem.onAfterUpdate -= UpdateMotion;
        }

        void UpdateMotion()
        {
            if (virtualMouse == null || Gamepad.current == null) return;

            Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;

            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            bool aButtonisPressed = Gamepad.current.aButton.IsPressed();
            if (previousMouseState != aButtonisPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonisPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = aButtonisPressed;

                AnchorCursor(newPosition);
            }



        }
        void AnchorCursor(Vector2 position)
        {
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
            cursorTransform.position = anchoredPosition;
        }
    }


}
