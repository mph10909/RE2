using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class WeaponAnimations : MonoBehaviour, IFireable
{
    [SerializeField] Transform weaponSlot;


    public void WeaponFire()
    {
        ExecuteActionOnFireables(fireable => fireable.WeaponFire());
    }


    public void CasingEject()
    {
        ExecuteActionOnFireables(fireable => fireable.CasingEject());
    }

    public void WeaponReload()
    {
        ExecuteActionOnFireables(fireable => fireable.WeaponReload());
    }

    public void WeaponReloadPart2()
    {
        ExecuteActionOnFireables(fireable => fireable.WeaponReload());
    }

    private void ExecuteActionOnFireables(System.Action<IFireable> action)
    {
        foreach (Transform child in weaponSlot)
        {
            var fireable = child.GetComponent<IFireable>();
            if (fireable != null)
            {
                
                action(fireable);
            }
        }
    }

    public bool AIFire()
    {
        return false;
    }

    public bool IsAmmoEmpty()
    {
        return false;
    }
}
