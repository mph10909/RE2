using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ClearInventorySelections : MonoBehaviour
    {

        [SerializeField] Transform inventoryButtons;

        public void OnClickUnSelectInventoryItems()
        {
            UnSelect();
        }

        public void UnSelect()
        {
            foreach (Transform child in inventoryButtons)
            {
                if (child.name == "ItemTemplate") continue;
                else
                {
                    IUnselectable unSelect = child.gameObject.GetComponent<IUnselectable>();
                    unSelect.UnSelect();
                }

            }
        }
    }
}
