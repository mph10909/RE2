using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void WeaponFire();
    void WeaponReload();
    void CasingEject();
    bool AIFire();
    bool IsAmmoEmpty();
}
