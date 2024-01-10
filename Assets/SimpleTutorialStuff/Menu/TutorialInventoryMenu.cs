using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class TutorialInventoryMenu : MonoBehaviour
{

    [SerializeField] Transform itemSlotContainer, itemSlotTemplate;
    [SerializeField] float itemSlotCellSizeX = 75;
    [SerializeField] float itemSlotCellSizeY = -40;

    public GameObject popUpMenu;
    public Action<TutorialItem> OnUseItem;
    public TutorialPlayerInventory inventory;
    public TutorialCombineItemsManager combineManager;
    private Button selectedButton;

    [SerializeField]bool firstItemSet;
    bool isCombineItemRunning = false;

    void OnEnable()
    {
        OnUseItem = UseInventoryItem;
        RefreshInventoryItems();
    }

    private void OnDisable()
    {
        popUpMenu.SetActive(false);
    }


    void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        int id = 0;

        foreach (TutorialItem item in inventory.GetItems)
        {
            RectTransform itemSlotRectTransfrom = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransfrom.gameObject.SetActive(true);
            itemSlotRectTransfrom.gameObject.name = id.ToString();
            itemSlotRectTransfrom.anchoredPosition = new Vector2(x * itemSlotCellSizeX, y * itemSlotCellSizeY);

            Image image = itemSlotRectTransfrom.GetComponent<Image>();

            image.sprite = item.GetSprite();
            x++;
            
            if (x > 6)
            {
                x = 0;
                y++;
            }

            Button button = itemSlotRectTransfrom.GetComponent<Button>();
            button.onClick.AddListener(() => ShowPopUpMenu(item, button));

            if (id == 0)
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
            }
            id++;
        }
    }

    void ShowPopUpMenu(TutorialItem itemName, Button button)
    {
        selectedButton = button;
        popUpMenu.SetActive(true);

        Button useButton = popUpMenu.transform.Find("UseButton").GetComponent<Button>();
        useButton.onClick.AddListener(() => UseItem(itemName));

        Button cancelButton = popUpMenu.transform.Find("CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(ClosePopUpMenu);

        //Button combineButton = popUpMenu.transform.Find("CombineButton").GetComponent<Button>();
        //combineButton.onClick.AddListener(() => CombineItem(itemName, button));

        EventSystem.current.SetSelectedGameObject(useButton.gameObject);
    }

    void UseItem(TutorialItem item)
    {
        if (OnUseItem != null)
        {
            OnUseItem.Invoke(item);
        }
    }

    private void UseInventoryItem(TutorialItem obj)
    {
        print(obj.item.ToString());
        inventory.RemoveItem(obj);
        RefreshInventoryItems();
        ClosePopUpMenu();
    }



    void ClosePopUpMenu()
    {
        popUpMenu.SetActive(false);
        selectedButton.Select();
    }


}




