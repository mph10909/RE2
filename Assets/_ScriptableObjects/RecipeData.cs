using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New RecipeData", menuName = "Recipe Data")]
public class RecipeData: ScriptableObject
{
    public Item.ItemType firstItem;
    public Item.ItemType secondItem;
    public Item combineResult;
    public bool reloadable;
    public bool combinable;
}
