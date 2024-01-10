using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledOnPause : MonoBehaviour
{
    [SerializeField] GameObject[] PausedObject; 

    // Update is called once per frame
    void Update()
    {
        if(Inventory.gameIsPaused) {foreach(GameObject objectToBePaused in PausedObject) objectToBePaused.SetActive(false);}
        else {foreach(GameObject objectToBePaused in PausedObject) objectToBePaused.SetActive(true);}

    }
}
