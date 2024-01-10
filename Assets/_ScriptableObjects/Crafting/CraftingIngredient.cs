using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Item", menuName = "Crafting/Crafting Item")]
public class CraftingIngredient : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool stackable;
}
