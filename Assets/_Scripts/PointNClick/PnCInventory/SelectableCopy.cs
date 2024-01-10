using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class SelectableCopy : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IUnselectable
    {
        public bool combining;
        [HideInInspector] public Item thisItem;
        [SerializeField]  GameObject itemMenu;
        [SerializeField]  Text itemName, equipUseButton;
        [SerializeField]  CombineItemButton combiningItem;

        Transform  surround, selectedSurround, items;
        SelectableCopy selectable;
        Button thisButton;

        [SerializeField] bool itemSelected;

        void Awake()
        {
            selectable = GetComponent<SelectableCopy>();
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

        public void OnPointerEnter(PointerEventData eventdata)
        {
            Highlighted();
        }

        public void OnPointerExit(PointerEventData eventdata)
        {
            Deselected();
        }

        public void OnSelect(BaseEventData eventdata)
        {
            Highlighted();
        }

        public void OnDeselect(BaseEventData eventdata)
        {
            Deselected();
        }

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
                item.gameObject.GetComponent<SelectableCopy>().itemSelected = true;
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
            //this.gameObject.GetComponent<Selectable>().combining = false;
            //this.gameObject.GetComponent<Selectable>().itemSelected = false;
            //this.gameObject.GetComponent<Selectable>().selectedSurround.gameObject.SetActive(false);
            //this.gameObject.GetComponent<Selectable>().surround.gameObject.SetActive(false);
            //this.gameObject.GetComponent<Button>().enabled = true;
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
            //foreach (Transform item in items)
            //{
            //    if (item.name == "ItemTemplate") continue;
            //    item.gameObject.GetComponent<Selectable>().combining = false;
            //    item.gameObject.GetComponent<Selectable>().itemSelected = false;
            //    item.gameObject.GetComponent<Selectable>().selectedSurround.gameObject.SetActive(false);
            //    item.gameObject.GetComponent<Selectable>().surround.gameObject.SetActive(false);
            //    item.gameObject.GetComponent<Button>().enabled = true;
            //    itemName.text = "";

            //}
            foreach (Transform child in items)
            {
                if (child.name == "ItemTemplate") continue;
                else
                {
                    IUnselectable unSelect = child.gameObject.GetComponent<IUnselectable>();
                    unSelect.UnSelect();
                }

            }
            //  itemMenu.SetActive(false);
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
