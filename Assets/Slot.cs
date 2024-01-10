using UnityEngine;
using UnityEngine.UI;

namespace UICollecter
{
    [System.Serializable]
    public class Slot : MonoBehaviour
    {
        public Image uiImage;
        public bool isOccupied = false;
        public CollectibleItem itemData;
        public int itemsInTransit = 0;
        public int itemCount = 0;
        public int maxCount = 5; // Maximum number of items that can be stacked
        public Text itemCountText;

        public void UpdateSlotUI()
        {
            if (itemCount > 0 && itemData != null)
            {
                uiImage.gameObject.SetActive(true);
                uiImage.sprite = itemData.itemSprite;
                itemCountText.text = itemCount > 1 ? itemCount.ToString() : "";
            }
            else
            {
                uiImage.sprite = null;
                itemCountText.text = "";
            }
        }
    }
}
