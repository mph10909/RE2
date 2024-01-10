using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Item", menuName = "Crafting/Ammo/Crafting Item")]
public class AmmoItem : CraftingIngredient
{
    public WeaponItem weaponType;
}
