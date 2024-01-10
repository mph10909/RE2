using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New WeaponDataBase", menuName = "ResidentEvilClone/Weapon Data/DataBase")]
public class WeaponDB : ScriptableObject
{
    public List<WeaponEntry> weapons;

    [System.Serializable]
    public class WeaponEntry
    {
        public Item.ItemType weaponType;
        public WeaponData weaponData;
    }
}

