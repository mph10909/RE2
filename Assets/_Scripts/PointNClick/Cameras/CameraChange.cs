using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] Transform camera1;
    [SerializeField] Transform camera2;
    [SerializeField] GameObject characterCamera; 

    void OnEnable()
    {
        Actions.SetCamera += SetCamera;
    }

    void OnDisable()
    {
        Actions.SetCamera -= SetCamera;
    }

    void OnTriggerEnter(Collider other)
    {
        if(characterCamera.transform.position == camera1.position)
        {
            characterCamera.transform.position = camera2.position;
            characterCamera.transform.rotation = camera2.rotation;
        }
        else
        {
            characterCamera.transform.position = camera1.position;
            characterCamera.transform.rotation = camera1.rotation;
        }

    }

    void SetCamera(GameObject newCamera)
    {
        characterCamera = newCamera;
    }
}
