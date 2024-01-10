using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_inventory: MonoBehaviour
{

    public static InventoryTest inventory;
    [SerializeField] GameObject Menu, itemMenu;
    [SerializeField] Image ItemMenuUI, combineSpriteHolder , boarder, weaponSprite;
    private Transform itemSlotContainer;
    private Transform itemSpriteContainer;
    private Transform itemSlotTemplate;
    private Transform itemSpriteTemplate;
    public Transform weaponSlotContainer; // Transform Located on Character with weapon Gameobjects
    public Transform selectedItem;
    public Transform returnButton;
    private Transform descriptionText, itemsText, checkSprite, combineBoarder, combineSprite, firstCombinable, secondCombinable;

    [System.NonSerialized]public bool returnSelected, weaponSelected, usableSelected, checkingItem, firstCombinableSelected;
    
    [SerializeField] private int _listCount;
    [SerializeField] public int _listHeight = 3;
    [SerializeField] public  static int _lowerList ;

    [SerializeField] Item firstSelectedCombinable;
    [SerializeField] Item secondSelectedCombinable;
    [SerializeField] Item combinedItem;

    [Header("Weapons")]
    [SerializeField] GameObject handGunA;
    [SerializeField] GameObject shotGun;
    [SerializeField] GameObject grenadeGun;
    [SerializeField] GameObject magnum; 
       
    public event EventHandler OnGrenadeChanged;
    public event EventHandler OnCombinedItems;

    void Awake() {
        OnCombinedItems += NoCombinables;
        inventory = new InventoryTest(UseItem);
        SetInventory(inventory);
        itemSlotContainer     = transform.Find("ItemSlotContainer");
        itemSpriteContainer   = transform.Find("ItemSpriteContainer");
        itemSlotTemplate      = itemSlotContainer.Find("ItemTemplate");
        itemSpriteTemplate    = itemSpriteContainer.Find("ItemSpriteTemplate");
        returnButton          = itemSlotContainer.Find("Return");
        returnSelected = true;
        itemMenu.SetActive(false);
        Menu.SetActive(false);
        inventory.AddItem(new Item { itemType = Item.ItemType.Handgun_A, amount = 1 });
        inventory.AddItem(new Item { itemType = Item.ItemType.Shotgun, amount = 1 });
        inventory.AddItem(new Item { itemType = Item.ItemType.Handgun_Bullets, amount = 30 });
        inventory.AddItem(new Item { itemType = Item.ItemType.Shotgun_Bullets, amount = 10 });
    }

    void OnEnable() {

        if(inventory == null) return;
        returnSelected = true;
        selectedItem = returnButton;
        returnButton.localPosition = new Vector2(0,0);
        RefreshInventoryItems();
    }
    
    private void UseItem(Item item){
    
    }

    void Update()
    {
        ItemLooper();
    }

    public void SetInventory (InventoryTest inventory){
        //this.inventory = inventory;
        UI_inventory.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();    
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e){
        //RefreshInventoryItems();
    }

    //Sets Everything Up For Inventory Display
    private void RefreshInventoryItems(){
        returnButton.localPosition = new Vector2(0,0);
        if(inventory == null) return;
        if(itemSpriteContainer == null)return;
        if(itemSlotContainer == null)return;
        foreach (Transform child in itemSpriteContainer){
            if(child == itemSpriteTemplate) continue;
            Destroy(child.gameObject);
        }        
        foreach (Transform child in itemSlotContainer){
            if(child == itemSlotTemplate) continue;
            if(child == returnButton) continue;
            Destroy(child.gameObject);
        }

        int x = InventoryTest.ListCount * -1;
        _lowerList = x + _listHeight;
        int y = _lowerList; 
        float itemSlotCellSize = 20f;
        float itemSlotCellSize2 = 40f;  
        foreach (Item item in inventory.GetItemList()){
            RectTransform itemSpriteRectTransfrom = Instantiate(itemSpriteTemplate, itemSpriteContainer).GetComponent<RectTransform>();
            RectTransform itemSlotRectTransfrom = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransfrom.gameObject.SetActive(true);
            itemSpriteRectTransfrom.gameObject.SetActive(true);
            itemSlotRectTransfrom.anchoredPosition = new Vector2(0, y * itemSlotCellSize);
            itemSpriteRectTransfrom.anchoredPosition = new Vector2(0, y * itemSlotCellSize2);            
            
            Text      amountText      = itemSpriteRectTransfrom.Find("AmountText").GetComponent<Text>();           
            Text      descriptionText = itemSlotRectTransfrom.Find("DescriptionText").GetComponent<Text>();
            Text      itemText        = itemSlotRectTransfrom.Find("ItemText").GetComponent<Text>();
            Image     image           = itemSpriteRectTransfrom.Find("ItemImage").GetComponent<Image>();      
            Image     checkImage      = itemSlotRectTransfrom.Find("CheckSprite").GetComponent<Image>();
            Image     combineImage    = itemSlotRectTransfrom.Find("CombineSprite").GetComponent<Image>();
            ItemWorld weaponType      = itemSlotRectTransfrom.Find("Equipable").GetComponent<ItemWorld>();
            ItemWorld itemType        = itemSlotRectTransfrom.Find("Usable").GetComponent<ItemWorld>();            
            
            if(item.IsEquipable()){
                weaponType.SetItem(item);
                }else{Destroy(weaponType.gameObject);}
            
            if(item.IsUsable()){
                itemType.SetItem(item);
                }else{Destroy(itemType.gameObject);}            
            
            itemText.text = item.GetText();            
            descriptionText.text = item.GetDescriptionText();
            checkImage.sprite = item.GetCheckSprite();
            image.sprite = item.GetSprite();
            combineImage.sprite = item.GetSprite();            
            
            if(item.amount > 1){
                amountText.text = item.amount.ToString();
            }else{
                amountText.text = "";
                }            
            y++;

            if(y==0){
                y++;
                }            
        }        
    }    

    //Move Inventory Item Up
    public void InventoryItemsUpwards(){
            ComponentController.inventoryItemsMoving = true;

            foreach (Transform child in itemSpriteContainer){
            if(child == itemSpriteTemplate) continue;
            //Tween.Move(child.gameObject, new Vector3(child.position.x, child.position.y + 18, child.position.z), 0.5f, ()=> ItemLooper());
            
        }            
            foreach (Transform child in itemSlotContainer){
            if (child == itemSlotTemplate) continue;
            ILoopable loopable = child.gameObject.GetComponent<ILoopable>();
            //Tween.Move(child.gameObject, new Vector3(child.position.x, child.position.y + 9, child.position.z), 0.5f, ()=> ItemLooper());            
        }

    }

    // Move Inventory Item Down
    public void InventoryItemsDownWards(){

            ComponentController.inventoryItemsMoving = true;

            foreach (Transform child in itemSpriteContainer){
            if(child == itemSpriteTemplate) continue;
           // Tween.Move(child.gameObject, new Vector3(child.position.x, child.position.y -18, child.position.z), 0.5f, ()=> ItemLooper());
        }            
            foreach (Transform child in itemSlotContainer){
            if(child == itemSlotTemplate) continue;
            ILoopable loopable = child.gameObject.GetComponent<ILoopable>();
            if (loopable == null) Debug.Log("No Loop");
            //Tween.Move(child.gameObject, new Vector3(child.position.x, child.position.y - 9, child.position.z), 0.5f, ()=> ItemLooper());
        }

    }

    private void ItemLooper(){ 

            foreach (Transform child in itemSpriteContainer){
            if (child.localPosition.y > 158){

                    child.localPosition = new Vector2(child.localPosition.x, _lowerList * 40);
                    }

            if(child.localPosition.y < _lowerList * 40){
                    child.localPosition = new Vector2(child.localPosition.x, _listHeight * 40);
                }
            }

            foreach (Transform child in itemSlotContainer){
            if (child.localPosition.y > 78){
                    child.localPosition = new Vector2(child.localPosition.x, _lowerList * 20);
                }

            if (child.localPosition.y < _lowerList * 20){
                    child.localPosition = new Vector2(child.localPosition.x, _listHeight * 20);
               
                } 
            }
                        
            ColorChanger();

    }
    
    private void ColorChanger(){
            foreach (Transform item in itemSlotContainer){
            if(item == itemSlotTemplate) continue;
                 
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                if(item == returnButton)   {returnSelected = true; usableSelected = false; weaponSelected = false; return;}
                if(item.Find("Equipable")) {weaponSelected = true; usableSelected = false; returnSelected = false;}
                if(item.Find("Usable"))    {usableSelected = true; weaponSelected = false; returnSelected = false;} 
                
                item.Find("ItemText").GetComponent<Text>().color = new Color32(0,255,16,255);//Color Changer 
                }else{
                if(item == returnButton)continue;
                item.Find("ItemText").GetComponent<Text>().color = new Color32(255,255,255,255);//Color Change Back
                }    
            
            }
    }

    //Information On an Item
     public void CheckItemInfo(){
        foreach(Transform item in itemSlotContainer){
            if(item == itemSlotTemplate) continue;
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                
                itemsText        = item.Find("ItemText");
                descriptionText  = item.Find("DescriptionText");
                checkSprite      = item.Find("CheckSprite");
                ItemMenuUI.enabled = false;
                boarder.enabled = false;
                itemsText.gameObject.SetActive(false);
                descriptionText.gameObject.SetActive(true);
                checkSprite.gameObject.SetActive(true);
                //Tween.Scale(checkSprite.gameObject, 100, 0.5f);

            }else{
                item.gameObject.SetActive(false);
            }
        }

    }

     public void CheckItemOff(){
        foreach(Transform item in itemSlotContainer){
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                
                
                itemsText         = item.Find("ItemText");
                descriptionText   = item.Find("DescriptionText");
                checkSprite       = item.Find("CheckSprite");
                //Tween.Scale(checkSprite.gameObject, 0.01f, 0.5f, ()=> 

                //{foreach(Transform item1 in itemSlotContainer){
                //    if(item1 == itemSlotTemplate) continue;
                //    item1.gameObject.SetActive(true);}
                //checkSprite.gameObject.SetActive(false);
                //descriptionText.gameObject.SetActive(false);
                //ItemMenuUI.enabled = enabled;
                //boarder.enabled = true;
                //itemsText.gameObject.SetActive(true);
                //});

            }
        }

    }

    // Combine Item Functions
    public void SelectCombinableItem(){
        foreach(Transform item in itemSlotContainer){
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                combineBoarder = item.Find("Boarder");
                combineSprite   = item.Find("CombineSprite");
                combineBoarder.gameObject.SetActive(true); 
                combineSpriteHolder.sprite = combineSprite.gameObject.GetComponent<Image>().sprite;
                combineSpriteHolder.gameObject.SetActive(true);

                firstSelectedCombinable = item.gameObject.GetComponentInChildren<ItemWorld>().GetItem();
                }
        }
    }

    public void SelectSecondCombinableItem(){
        foreach(Transform item in itemSlotContainer){
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                    secondSelectedCombinable = item.gameObject.GetComponentInChildren<ItemWorld>().GetItem();
                    CombineItems();
                }
            }
        }
    
    void CombineItemsDisapear(){
        foreach(Transform item in itemSlotContainer){
            //Tween.TextFade(item.gameObject.GetComponentInChildren<Text>(), 0, 0.25f, ()=> {
            //      foreach(Transform item1 in itemSlotContainer){
            //              item1.gameObject.SetActive(false);}
            //      foreach(Transform item1 in itemSpriteContainer){
            //              item1.gameObject.SetActive(false);}
            //      OnCombinedItems?.Invoke(this, EventArgs.Empty); return;});                  
        }

        foreach(Transform item in itemSpriteContainer){
            //Tween.Fade(item.gameObject.GetComponentInChildren<Image>(), 0, 0.25f);
            //Tween.TextFade(item.gameObject.GetComponentInChildren<Text>(), 0, 0.25f);
        }

    }

    void NoCombinables(object sender, EventArgs e){
        Debug.Log(firstSelectedCombinable.itemType);
        inventory.AddItem(new Item {itemType = combinedItem.itemType, amount = 1});
        inventory.RemoveItem(firstSelectedCombinable);
        inventory.RemoveItem(secondSelectedCombinable);  
        firstSelectedCombinable.itemType = Item.ItemType.None;
        secondSelectedCombinable.itemType = Item.ItemType.None;
        RefreshInventoryItems();
        CombineItemsAppear();
        return;
    }

    void CombineItemsAppear(){
            
        foreach(Transform item in itemSlotContainer){
                if(item == itemSlotTemplate) continue;
                item.gameObject.SetActive(true);}
        foreach(Transform item in itemSpriteContainer){
                if(item == itemSpriteTemplate) continue;
                item.gameObject.SetActive(true);}

        foreach(Transform item in itemSlotContainer){
            //Tween.TextFade(item.gameObject.GetComponentInChildren<Text>(), 1, 0.01f);
        }
        foreach(Transform item in itemSpriteContainer){
            //Tween.Fade(item.gameObject.GetComponentInChildren<Image>(), 1, 0.01f);
            //Tween.TextFade(item.gameObject.GetComponentInChildren<Text>(), 1, 0.01f);
        }
    }
        
    // Equip Weapon Function
    public void WeaponEquip(){
        Item weaponToEquip;
        Item weaponBeingSelected;
        Transform reloading = weaponSlotContainer.Find("Reloading");
        Transform hud = weaponSlotContainer.Find("Hud");
        Transform crossHair = weaponSlotContainer.Find("CrossHair");
        Transform crossHair2 = weaponSlotContainer.Find("CrossHairReverse");

        foreach(Transform item in itemSlotContainer){
            if(item.localPosition.y > -0.2f && item.localPosition.y < 0.2f){
                weaponBeingSelected = item.GetComponentInChildren<ItemWorld>().GetItem();
                    foreach(Transform weapon in weaponSlotContainer){
                    if(weapon ==  reloading) continue;
                    if(weapon ==  hud) continue;
                    if(weapon ==  crossHair) continue;
                    if(weapon ==  crossHair2) continue;
                    weaponToEquip = weapon.GetComponent<ItemWorld>().GetItem();
                    if(weaponToEquip.itemType == weaponBeingSelected.itemType){
                        Debug.Log(weaponBeingSelected.itemType + " Is the weapon to be equiped");
                        weapon.gameObject.SetActive(true);
                        }else {weapon.gameObject.SetActive(false);}
                    }        
                }             
        }

        SetWeaponSprite();
    }

    public void SetWeaponSprite(){
            
        foreach(Transform weapon in weaponSlotContainer){
            Item weaponToEquip;
            Transform reloading = weaponSlotContainer.Find("Reloading");
            Transform hud = weaponSlotContainer.Find("Hud");
            Transform crossHair = weaponSlotContainer.Find("CrossHair");
            Transform crossHair2 = weaponSlotContainer.Find("CrossHairReverse");

            //Image weaponSprite =  weaponSpriteTemplate.Find("WeaponImage").GetComponent<Image>();
            if(weapon.gameObject.activeSelf){
                if(weapon ==  reloading) continue;
                if(weapon ==  hud) continue;
                if(weapon ==  crossHair) continue;
                if(weapon ==  crossHair2) continue;
                weaponToEquip = weapon.GetComponent<ItemWorld>().GetItem();
                weaponSprite.sprite = weaponToEquip.GetSprite();
            }
        }
    }
    
    public void CombineItems(){
        foreach(Transform item in itemSlotContainer){
            combineBoarder = item.Find("Boarder");
            combineBoarder.gameObject.SetActive(false);
            combineSpriteHolder.gameObject.SetActive(false); 
        }
        //Grenade Ammo Swap
        if((firstSelectedCombinable.itemType == Item.ItemType.Grenade_Gun   || secondSelectedCombinable.itemType == Item.ItemType.Grenade_Gun)  &&  
           (firstSelectedCombinable.itemType  == Item.ItemType.Flame_Rounds || secondSelectedCombinable.itemType  == Item.ItemType.Flame_Rounds)){
               //grenadeGun.GetComponent<GunController>().ammoType.itemType = Item.ItemType.Flame_Rounds;
               OnGrenadeChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Flame_Equip");
        }
        if((firstSelectedCombinable.itemType == Item.ItemType.Grenade_Gun  || secondSelectedCombinable.itemType == Item.ItemType.Grenade_Gun)  &&  
           (firstSelectedCombinable.itemType  == Item.ItemType.Acid_Rounds || secondSelectedCombinable.itemType  == Item.ItemType.Acid_Rounds)){
               //grenadeGun.GetComponent<GunController>().ammoType.itemType = Item.ItemType.Acid_Rounds;
               OnGrenadeChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Acid_Equip");
        }
        if((firstSelectedCombinable.itemType == Item.ItemType.Grenade_Gun     || secondSelectedCombinable.itemType == Item.ItemType.Grenade_Gun)  &&  
           (firstSelectedCombinable.itemType  == Item.ItemType.Grenade_Rounds || secondSelectedCombinable.itemType  == Item.ItemType.Grenade_Rounds)){
               //grenadeGun.GetComponent<GunController>().ammoType.itemType = Item.ItemType.Grenade_Rounds;
               OnGrenadeChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Grenade_Equip");
        }
        //End Grenade Ammo Swap//

        //Mixing Herbs//
        
        //Mixed Herb G + G//
        if(firstSelectedCombinable.itemType == Item.ItemType.Green_Herb && secondSelectedCombinable.itemType == Item.ItemType.Green_Herb){
            Debug.Log("G + G Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GG;
            CombineItemsDisapear();
            return;
        }
        //Mixed Herb G + R//
        if((firstSelectedCombinable.itemType == Item.ItemType.Green_Herb && secondSelectedCombinable.itemType == Item.ItemType.Red_Herb)  ||  
           (firstSelectedCombinable.itemType  == Item.ItemType.Red_Herb  && secondSelectedCombinable.itemType  == Item.ItemType.Green_Herb)){
            Debug.Log("G + R Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GR;
            CombineItemsDisapear();
            return;
        }
        //Mixed Herb G + B//
        if((firstSelectedCombinable.itemType == Item.ItemType.Blue_Herb   && secondSelectedCombinable.itemType == Item.ItemType.Green_Herb)  ||  
           (firstSelectedCombinable.itemType  == Item.ItemType.Green_Herb && secondSelectedCombinable.itemType  == Item.ItemType.Blue_Herb)){
            Debug.Log("G + B Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GB;
            CombineItemsDisapear();
            return;
        }
        //Mixed Herb G + G + B//
        if((firstSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GB && secondSelectedCombinable.itemType == Item.ItemType.Green_Herb)     ||  
           (firstSelectedCombinable.itemType == Item.ItemType.Green_Herb    && secondSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GB)  ||
           (firstSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GG && secondSelectedCombinable.itemType == Item.ItemType.Blue_Herb)      ||
           (firstSelectedCombinable.itemType == Item.ItemType.Blue_Herb     && secondSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GG)) {
            Debug.Log("G + G + B Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GGB;
            CombineItemsDisapear();
            return;
        }
        
        //Mixed Herb G + R + B//
        if((firstSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GB && secondSelectedCombinable.itemType == Item.ItemType.Red_Herb)       ||  
           (firstSelectedCombinable.itemType == Item.ItemType.Red_Herb      && secondSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GB)  ||
           (firstSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GR && secondSelectedCombinable.itemType == Item.ItemType.Blue_Herb)      ||
           (firstSelectedCombinable.itemType == Item.ItemType.Blue_Herb     && secondSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GR)) {
            Debug.Log("G + R + B Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GRB;
            CombineItemsDisapear();
            return;
        }

        //Mixed Herb G + G + G//
        if((firstSelectedCombinable.itemType == Item.ItemType.Green_Herb && secondSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GG) ||
          (firstSelectedCombinable.itemType == Item.ItemType.Mixed_Herb_GG && secondSelectedCombinable.itemType == Item.ItemType.Green_Herb)) {
            Debug.Log("G + G Mixed Herb Made");
            combinedItem.itemType = Item.ItemType.Mixed_Herb_GG;
            CombineItemsDisapear();
            return;
        }



    }
}
