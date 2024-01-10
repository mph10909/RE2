using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BreakerPuzzle : DisplayText, IInteractable
    {

        [SerializeField] GameObject breakerPuzzle;
        [SerializeField] string puzzleSolvedText;
        bool puzzleSolved;

        public void Interact()
        {
            if (!puzzleSolved)
            {
                Instantiate(breakerPuzzle, new Vector3(0, 0, 0), Quaternion.identity);
                Time.timeScale = 0;
                Cursor.visible = false;
                GameStateManager.Instance.SetState(GameState.Paused); 
            }
            else
            {
                TextDisplay(puzzleSolvedText);
            }
        }
    }
}
