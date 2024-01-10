using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class PncInventory : MonoBehaviour
    {
        [SerializeField] GameObject checkMenu;
        [SerializeField] Image profilePicture, healthBar, condition;

        [SerializeField] Transform itemSlotContainer,itemSlotTemplate;

        float itemSlotCellSizeX = 75;
        float itemSlotCellSizeY = -40;

        [HideInInspector]public PlayerInventory inventory;
        [HideInInspector]public AmmoInventory ammoInventory;

        PlayerAttacked healthColors;
        private IUIInput m_Input;

        private void Awake()
        {
            m_Input = GetComponentInParent<IUIInput>();
            this.gameObject.SetActive(false);
        }

        void Update()
        {
            if (m_Input.IsCancelDown() || m_Input.IsToggleInventoryDown())
            {
                OnCancel();
            }
        }

        private void OnCancel()
        {
            if (checkMenu.activeSelf)
            {
                checkMenu.SetActive(false);
            }
            else
            {
                Hide();
            }
        }

        private void Hide()
        {
            PauseController.Instance.Resume();
            checkMenu.SetActive(false);
            gameObject.SetActive(false);
        }

        void SetInventory(GameObject selectedPlayer)
        {
            healthColors = selectedPlayer.GetComponent<PlayerAttacked>();
            inventory = selectedPlayer.GetComponent<PlayerInventory>();
            ammoInventory = selectedPlayer.GetComponent<AmmoInventory>();
        }

        internal void Show()
        {
            PauseController.Instance.Pause();
            this.gameObject.SetActive(true);
            checkMenu.SetActive(false);
        }

        void OnEnable()
        {
            Actions.RefreshInventory += RefreshInventoryItems;
            SetInventory(CharacterManagement.Instance.currentPlayer);
            profilePicture.sprite = CharacterManagement.Instance.characterName.CharacterSprite();
            RemoveEmptyItems();
            RefreshInventoryItems();
        }

        void OnDisable()
        {
            Actions.RefreshInventory -= RefreshInventoryItems;
            PauseController.Instance.Resume();
        }

        void RefreshInventoryItems()
        {
            healthBar.color = healthColors.healthStats.HealthColors();
            condition.sprite = healthColors.healthStats.HealthCondition();

            foreach (Transform child in itemSlotContainer){
                if (child == itemSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;
            int id = 0;

            foreach (Item item in inventory.inventory.GetItemList())
            {
                RectTransform itemSlotRectTransfrom = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransfrom.gameObject.name = item.itemType.ToString() + "_" + id;
                itemSlotRectTransfrom.gameObject.SetActive(true);
                itemSlotRectTransfrom.anchoredPosition = new Vector2(x * itemSlotCellSizeX, y * itemSlotCellSizeY);

                Image image = itemSlotRectTransfrom.GetComponent<Image>();
                Text nameText = itemSlotRectTransfrom.Find("NameText").GetComponent<Text>();
                Text amountText = itemSlotRectTransfrom.Find("AmountText").GetComponent<Text>();

                itemSlotRectTransfrom.GetComponent<Selectable>().thisItem = item;

                nameText.text = item.GetText();
                image.sprite = item.GetSprite();

                if (item.amount > 1) amountText.text = item.amount.ToString();
                else amountText.text = "";

                if (item.IsWeapon()){
                    amountText.color = new Color(0, 112, 183);
                    amountText.text = WeaponAmmo(item);
                }
            
                id++;
                x++;

                if(x > 3){
                    x = 0;
                    y++;
                }
            }
        }

        string WeaponAmmo(Item item)
        {
            string loadedAmmo = "";
            foreach (AmmoInventory.AmmoEntry ammo in ammoInventory._inventory)
            {
                if (item.GetAmmo() == ammo.inventoryAmmo.itemType && item.itemType == ammo.weaponType)
                {
                    loadedAmmo = ammo.loaded.ToString();   
                }
            }return loadedAmmo;
        }

        void RemoveEmptyItems()
        {
            foreach (Item item in inventory.inventory.GetItemList())
            {
                if (item.IsStackable())
                {
                    if (item.amount == 0) inventory.inventory.RemoveItem(item);
                    return;
                }
            }

        }

    }
}
