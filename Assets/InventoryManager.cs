using UnityEngine;
using System.Collections.Generic;
namespace UICollecter
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        public List<Slot> slots;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public Slot FindAvailableSlot(CollectibleItem item)
        {
            Slot targetSlot = null;

            // Find a slot that already contains the same item type and isn't full
            foreach (var slot in slots)
            {
                if (slot.itemData == item && slot.itemCount < slot.maxCount)
                {
                    // Prefer a slot that's not currently occupied
                    if (!slot.isOccupied)
                    {
                        slot.isOccupied = true;
                        return slot;
                    }

                    // Keep track of a partially filled slot of the same type
                    if (targetSlot == null || slot.itemCount < targetSlot.itemCount)
                    {
                        targetSlot = slot;
                    }
                }
            }

            // Use a slot that is already in the process of being filled with the same item
            if (targetSlot != null)
            {
                return targetSlot;
            }

            // Find an empty slot if no partially filled slot is available
            foreach (var slot in slots)
            {
                if (!slot.isOccupied && slot.itemData == null)
                {
                    slot.isOccupied = true;
                    return slot;
                }
            }

            return null;
        }



        public void UpdateSlot(Slot slot, CollectibleItem item)
        {
            if (slot.itemData == null)
            {
                slot.itemData = item;
                slot.itemCount = 1;
            }
            else if (slot.itemData == item)
            {
                slot.itemCount++;
            }
            slot.UpdateSlotUI();
        }
    }
}