using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class CombineItemButton : MonoBehaviour
    {
        [SerializeField] Transform items;
        public Item firstItem, secondItem;
        [SerializeField] PncInventory inventory;


        void OnEnable()
        {
            Actions.CombineItem += CombineItems;
        }

        void OnDisable()
        {
            Actions.CombineItem -= CombineItems;
        }

        public void OnClick_Combine()
        {
            foreach(Transform item in items)
                {
                    if (item.name == "ItemTemplate") continue;
                    item.GetComponent<Button>().enabled = true;
                    item.GetComponent<Selectable>().combining = true;
                }                             
        }

        void CombineItems()
        {
            Actions.CombineItems?.Invoke(firstItem, secondItem);
        }

    }
}
