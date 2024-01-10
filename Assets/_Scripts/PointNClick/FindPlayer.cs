using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayer : MonoBehaviour
{
    GameObject cameraManager;
    GameObject characterManager;
    Transform characters;
    Transform cameras;
    public GameObject playerCamera;
    public GameObject player;
    [HideInInspector] public PointAndClick mouseClick;

    public void GetPlayer()
    {
        cameraManager = GameObject.Find("-------Cameras-------");
        characterManager = GameObject.Find("------Characters------");
        cameras = cameraManager.transform;
        characters = characterManager.transform;
        FindPlayerCharacter();
    }

    public void FindPlayerCharacter()
    {
        foreach(Transform playerChar in characters)
        {
            if (playerChar.gameObject.GetComponent<PointAndClick>().enabled)
            {
                player = playerChar.gameObject;
            }
        }

        foreach (Transform playerCam in cameras)
        {
            if (playerCam.gameObject.activeSelf)
            {
                playerCamera = playerCam.gameObject;
            }
        }
    }

}
