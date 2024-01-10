using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "ResidentEvilClone/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public float weaponNumber;
    public GameObject weaponObject;
    public Weapon weapon;
}

