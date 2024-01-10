using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public CraftingIngredient resultingItem;
    public CraftingItem[] ingredients;
}

[System.Serializable]
public class CraftingItem
{
    public CraftingIngredient item;
    public int quantity;
}
