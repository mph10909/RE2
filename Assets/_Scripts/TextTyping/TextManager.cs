using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class TextManager : MonoBehaviour
    {
        public void OnClick_TextClear(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
                {
                    Actions.TextClear?.Invoke();
                    return;
                }

            }
        }
    }
}
