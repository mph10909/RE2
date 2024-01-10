using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New InventoryData", menuName = "ResidentEvilClone/Inventory Data")]
public class InventoryData : ScriptableObject
{
    public Item[] startingInventory;
}