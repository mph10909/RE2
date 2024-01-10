using UnityEngine;

namespace UICollecter
{
    [CreateAssetMenu(fileName = "NewCollectibleItem", menuName = "Inventory/CollectibleItem")]
    public class CollectibleItem : ScriptableObject
    {
        public string itemName; // Unique identifier
        public Sprite itemSprite; // The sprite for the item
    }
}
