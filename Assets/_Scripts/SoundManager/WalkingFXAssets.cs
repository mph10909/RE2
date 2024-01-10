using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingFXAssets : MonoBehaviour
{
    public static WalkingFXAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Space(0.5f)][Header("Gravel")]
    public AudioClip[] footstepsGravel;

    [Space(0.5f)][Header("Wood")]
    public AudioClip[] footstepsWood;

    [Space(0.5f)][Header("Carpet")]  
    public AudioClip[] footstepsCarpet;

    [Space(0.5f)][Header("Stone")] 
    public AudioClip[] footstepsStone;

    [Space(0.5f)][Header("Grass")] 
    public AudioClip[] footstepsGrass;

    [Space(0.5f)][Header("Metal")]
    public AudioClip[] footstepsMetal;

    [Space(0.5f)][Header("Glass")]
    public AudioClip[] footstepsGlass;


}
