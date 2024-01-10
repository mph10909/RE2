using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAssets : MonoBehaviour
{

    public static WeaponAssets Instance {get; private set;}

    private void Awake(){
        Instance = this;
    }
    
    [Space(2)][Header("GrenadeGun")]
    public GameObject acidRoundSprite;
    public GameObject fireRoundSprite;
    public GameObject grenadeRoundSprite;

    public GameObject acidRound;
    public GameObject fireRound;
    public GameObject grenadeRound;
    
    public AudioClip  grenadeLaunch;
    public AudioClip  grenadeReload;
    public AudioClip  grenadeEmpty;
    
    [Space(2)][Header("HandGun")]
    
    public GameObject handgunRoundSprite;
    public AudioClip  handgunLaunch;
    public AudioClip  handgunReload;
    public AudioClip  handgunEmpty;

    [Space(2)][Header("ShotGun")]

    public GameObject shotgunRoundSprite;
    public AudioClip  shotgunLaunch;
    public AudioClip  shotgunReload;
    public AudioClip  shotgunEmpty;
}
