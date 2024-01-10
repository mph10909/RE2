using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeManger", menuName = "Crafting/Recipe Manager")]
public class RecipeManager : ScriptableObject
{
    public List<CraftingRecipe> craftingRecipes;
}


