using ResidentEvilClone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnableCharacterCamera : MonoBehaviour, IControllable
{
    [SerializeField] GameObject myCamera;
    public CharacterData character;

    public GameObject MyCamera { get { return myCamera; } set { myCamera = value; } }

    public void EnableControl(bool enable)
    {
        myCamera.SetActive(enable);
    }
}
