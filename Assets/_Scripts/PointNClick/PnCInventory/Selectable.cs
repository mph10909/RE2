using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class Selectable : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IUnselectable
    {
        public bool combining;
        [HideInInspector] public Item thisItem;
        [SerializeField]  GameObject itemMenu;
        [SerializeField]  Text itemName, equipUseButton;
        [SerializeField]  CombineItemButton combiningItem;

        Transform  surround, selectedSurround, items;
        Selectable selectable;
        Button thisButton;

        [SerializeField] bool itemSelected;

        void Awake()
        {
            selectable = GetComponent<Selectable>();
            thisButton = GetComponent<Button>();
            items = transform.parent;
            surround = transform.Find("Surround");
            selectedSurround = transform.Find("SelectedSurround");
        }

        void OnEnable()
        {
            ButtonReset();
        }
        
        void OnDisable()
        {
            selectedSurround.gameObject.SetActive(false);
            itemName.text = "";
            itemMenu.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventdata) { Highlighted(); }

        public void OnPointerExit(PointerEventData eventdata) { Deselected(); }

        public void OnSelect(BaseEventData eventdata) { Highlighted(); }

        public void OnDeselect(BaseEventData eventdata) { Deselected(); }

        void Deselected()
        {
            if (itemSelected && !combining) return;
            surround.gameObject.SetActive(false);
            itemName.text = "";
        }

        void Highlighted()
        {
            if (thisButton.enabled == false) return;
            surround.gameObject.SetActive(true);
            itemName.text = thisItem.GetText();
            SoundManagement.Instance.MenuSound(MenuSounds.Click);
        }

        public void OnClick_Selected()
        {
            SetItem(thisItem);

            if (combining) return;

            selectedSurround.gameObject.SetActive(true);
            foreach (Transform item in items)
            {
                if (item.name == "ItemTemplate") continue;
                item.gameObject.GetComponent<Selectable>().itemSelected = true;
                if (item.name != this.name)
                {
                    itemName.text = thisItem.GetText();
                    item.gameObject.GetComponent<Button>().enabled = false;
                    itemMenu.SetActive(true);
                    SetCombineItem(thisItem);     
                }
                if(items.childCount <= 2)
                {
                    print("One Item");
                    itemMenu.SetActive(true);
                }
            }
        }

        public void OnClick_SecondSelected()
        {
            if (!combining) return;
            SetCombineItem(thisItem);
            Actions.CombineItem?.Invoke();
            ButtonReset();
        }

        public void UnSelect()
        {
            selectable.combining = false;
            selectable.itemSelected = false;
            selectable.selectedSurround.gameObject.SetActive(false);
            selectable.surround.gameObject.SetActive(false);
            thisButton.enabled = true;
            itemName.text = "";
            itemMenu.SetActive(false);
        }

        void ButtonReset()
        {
            foreach (Transform child in items)
            {
                if (child.name == "ItemTemplate") continue;
                else
                {
                    IUnselectable unSelect = child.gameObject.GetComponent<IUnselectable>();
                    unSelect.UnSelect();
                }
            }
        }

        void SetItem(Item item)
        {
            if (!item.IsEquipable()) equipUseButton.text = "USE";
            else equipUseButton.text = "EQUIP";

            itemMenu.GetComponentInChildren<EquipWeapon>().item = item;
            itemMenu.GetComponentInChildren<CheckItem>().item = item;
        }

        void SetCombineItem(Item item)
        {
            if (!combining) combiningItem.firstItem = item;
            if (combining)  combiningItem.secondItem = item;
        }       
    }
}
