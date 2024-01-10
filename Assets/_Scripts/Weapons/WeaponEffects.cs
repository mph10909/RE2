using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class WeaponEffects : Weapon
{
    public WeaponConfigData weaponData;
    public WeaponStruct weaponEffects;
    public bool twoReload;

    [Header("Ammo Managment")]
    public AmmoInventory ammo;
    public AmmoType ammoType;
    public int ammoPerShot = 1;
    public int fullAmmo = 6;

    public const string RELOADING = "Reload";
    public const string RELOAD = "ReloadTrigger";

    public Animator anim;
    [HideInInspector] public bool isWeaponFireCalled = false;
    [HideInInspector] public bool isCasingCalled = false;

    public virtual void OnEnable()
    {
        ammo = GetComponentInParent<AmmoInventory>();
        anim = GetComponentInParent<Animator>();
        
    }

    public void WeaponFireEffects()
    {
        print("ShotsFired");
        weaponEffects.audioSource.PlayOneShot(weaponEffects.fire);
        if (weaponEffects.flashParticle != null) Instantiate(weaponEffects.flashParticle, weaponEffects.flashLocation.position, weaponEffects.flashLocation.rotation);
        if (weaponEffects.projectileParticle != null) Instantiate(weaponEffects.projectileParticle, weaponEffects.flashLocation.position, weaponEffects.flashLocation.rotation);      
    }

    public void CasingEject()
    {
        if (isCasingCalled) return;

        isCasingCalled = true;
        if (weaponEffects.casingParticle == null) return;

        Instantiate(weaponEffects.casingParticle, weaponEffects.casingLocation.position, weaponEffects.casingLocation.rotation);

        StartCoroutine(ResetCasingFlag());
    }

    public void WeaponEmpty()
    {
        weaponEffects.audioSource.PlayOneShot(weaponEffects.empty);
    }

    public override void WeaponReload()
    {
       { weaponEffects.audioSource.PlayOneShot(weaponEffects.reload);}
    }

    public void WeaponReloadPart2()
    {
        { weaponEffects.audioSource.PlayOneShot(weaponEffects.reload2); }
    }

    public Transform FindParentWithTag(Transform childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent;
            }
            t = t.parent.transform;
        }
        return null;
    }

    public void Reload()
    {
        anim.SetBool(RELOADING, true);
        ammo.Reload(ammoType, fullAmmo);
        anim.SetTrigger(RELOAD);
        return;
    }

    public IEnumerator ResetFireFlag()
    {
        yield return new WaitForSeconds(0.1f); // Example delay, adjust as needed
        isWeaponFireCalled = false;
    }

    public IEnumerator ResetCasingFlag()
    {
        yield return new WaitForSeconds(0.1f); // Example delay, adjust as needed
        isCasingCalled = false;
    }

}
