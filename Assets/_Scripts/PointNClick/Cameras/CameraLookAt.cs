using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    
    GameObject character;
    GameObject cameraSet;
    public bool cameraLook;
    public bool clampedCameraLook;
    [SerializeField] float minClamp;
    [SerializeField] float maxClamp;

    void OnEnable()
    {
        Actions.SetCamera += SetCamera;
        Actions.CharacterSwap += SetPlayer;
    }

    void OnDisable()
    {
        Actions.SetCamera += SetCamera;
        Actions.CharacterSwap -= SetPlayer;
    }

    void Update()
    {
        if (cameraLook)
        {
            CameraLook();
        }

        if (clampedCameraLook)
        {
            ClamperCamera();
        }
    }


    void CameraLook()
    {
        if(cameraSet != null) { 
        if (cameraSet.transform.position != this.transform.position) return;
            cameraSet.transform.LookAt(character.transform);}
    }

    void ClamperCamera()
    {
        if (cameraSet != null)
        {
            if (cameraSet.transform.position != this.transform.position) return;
            var direction = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z) - cameraSet.transform.position;
            var rotation = Quaternion.LookRotation(direction);

            rotation.eulerAngles = new Vector3(Mathf.Clamp(rotation.eulerAngles.x, minClamp, maxClamp), rotation.eulerAngles.y, rotation.eulerAngles.z);

            cameraSet.transform.rotation = rotation;
        }
    }

    void SetCamera(GameObject setCamera)
    {
        cameraSet = setCamera;
    }

    void SetPlayer(GameObject setCharacter)
    {
        character = setCharacter;
    }
}

