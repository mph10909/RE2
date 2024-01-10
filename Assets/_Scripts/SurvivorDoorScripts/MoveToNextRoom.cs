using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextRoom : MonoBehaviour
{
 [SerializeField] public GameObject Character , CharacterLoacation;
 [SerializeField] public float howFarToMove = 2.0f;

void Update()
 {
     if(DoorOpenScript.timeForCharacterTransition){
     Character.transform.rotation = CharacterLoacation.transform.rotation;
     Character.transform.position = CharacterLoacation.transform.position;
     DoorOpenScript.timeForCharacterTransition = false;
     }
 }
}
