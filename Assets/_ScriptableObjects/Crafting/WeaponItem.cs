using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Crafting/Crafting Item/Weapon")]
public class WeaponItem : CraftingIngredient
{
    public AudioClip fire;
    public AudioClip reload;
    public AudioClip empty;
    public int maxAmmo;
    public AmmoItem ammoType;
}
