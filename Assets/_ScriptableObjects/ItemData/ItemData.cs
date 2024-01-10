using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public Item.ItemType item;
    public string itemNameText;
    [TextArea(3, 10)]
    public string itemInventoryDescription;

    public Sprite itemInventorySprite;
    public Sprite itemCheckSprite;

    public GameObject itemInventoryGameObject;
}