using System.Collections.Generic;
using UnityEngine;

namespace TutorialStuff
{

    [CreateAssetMenu(fileName = "ItemRecipes", menuName = "Custom/Item Recipe")]
    public class ItemRecipe : ScriptableObject
    {
        [SerializeField]
        private List<ItemRecipes> recipes;

        public bool CanCombine(ItemType item1, ItemType item2)
        {
            foreach (ItemRecipes recipe in recipes)
            {
                if (recipe.CanCombine(item1, item2))
                {
                    return true;
                }
            }
            return false;
        }

        public ItemType Combine(ItemType item1, ItemType item2)
        {
            foreach (ItemRecipes recipe in recipes)
            {
                if (recipe.CanCombine(item1, item2))
                {
                    return recipe.Combine();
                }
            }
            return ItemType.None;
        }
    }

    public enum ItemType
    {
        None,
        RedHerb,
        GreenHerb,
        MixedHerb,
    }

    [System.Serializable]


    public struct ItemRecipes
    {
        public ItemType item1;
        public ItemType item2;
        public ItemType result;

        public bool CanCombine(ItemType item1, ItemType item2)
        {
            return (this.item1 == item1 && this.item2 == item2) || (this.item1 == item2 && this.item2 == item1);
        }

        public ItemType Combine()
        {
            return result;
        }
    }
}