using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAsset : MonoBehaviour
{
    public static WeaponsAsset Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public WeaponDB weaponDatabase;

    public WeaponData GetWeaponData(Item.ItemType weaponType)
    {
        foreach (var entry in weaponDatabase.weapons)
        {
            if (entry.weaponType == weaponType)
            {
                return entry.weaponData;
            }
        }
        return null;
    }
}
