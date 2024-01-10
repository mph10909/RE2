using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponStruct
{
    
    public AudioSource audioSource;
    public AudioClip fire;
    public AudioClip empty;
    public AudioClip reload;
    public AudioClip reload2;
    public GameObject flashParticle;
    public GameObject projectileParticle;
    public GameObject casingParticle;
    public Transform casingLocation;
    public Transform flashLocation;

}
