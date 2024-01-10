using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTrigger : MonoBehaviour, IUsable
{
    [SerializeField] NumberPad numberPad;
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject safePuzzle;


    public void ActivateObject()
    {
        //if(!numberPad.safeOpen && MouseLook.DoorRay) {
            Inventory.PAUSE();
            mainCharacter.SetActive(false);
            safePuzzle.SetActive(true);
        //}

    }

    public void PuzzleEnded() {
        Inventory.UNPAUSE();
        mainCharacter.SetActive(true);
        safePuzzle.SetActive(false);

    }

}
