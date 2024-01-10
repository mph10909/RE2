using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public virtual bool AIFire()
    {
        return false;
    }

    public virtual bool IsAmmoEmpty()
    {
        return false;
    }

    public virtual void WeaponFire()
    {
    }

    public virtual void WeaponReload()
    {
    }

    public virtual void Firing()
    {
    }
}