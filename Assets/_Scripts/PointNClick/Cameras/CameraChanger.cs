using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] Transform enterCam;
    [SerializeField] GameObject characterCamera;
    private CharacterManager charManager;

    void OnEnable()
    {
        Actions.SetCamera += SetCamera;
    }

    void OnDisable()
    {
        Actions.SetCamera -= SetCamera;
    }

    void Awake()
    {
            charManager = FindObjectOfType<CharacterManager>();
            characterCamera = charManager.currentCamera;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is not on the IgnoreRaycast layer
        if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            characterCamera.transform.position = enterCam.position;
            characterCamera.transform.rotation = enterCam.rotation;
        }
    }

    void SetCamera(GameObject newCamera)
    {
        characterCamera = newCamera;
    }

}
