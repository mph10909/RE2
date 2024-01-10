using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{   
    [SerializeField] int mainMenuInt, itemMenuInt;    
    [SerializeField] bool[] mainMenuActive, itemMenuActive;
    
    [SerializeField] GameObject playerCamera, PSXCamera, fullMenu, characterController;
    
    
    [SerializeField] GameObject mainMenu, selectionMenu, secondaryMenu, itemMenu, fileMenu, mapMenu;
    
    
    [SerializeField] GameObject[] mainMenuItems, itemMenuItems;
    [SerializeField] Sprite[] unSelected, HighLighted, Selected, itemunSelected, itemHighLighted, itemSelected, Use, Equip;
    
   
    [System.NonSerialized]public bool MenuItemSelected, ItemMenuActive, CheckItemActive, CombineItemActive, SecondaryMenuSelected;
    public static bool FileMenuActive, MapMenuActive;

    
    [SerializeField] Inventory inventory;
    [SerializeField] UI_inventory uiInventory;

    void Awake()
    {
        characterController = GameObject.FindWithTag("Player");
    }

    void Start(){
        
        characterController.GetComponent<WalkingSFX>().enabled = false;
        MenuButtonStart();
    }

    void OnEnable()
    {
        characterController.GetComponent<WalkingSFX>().enabled = false;
        uiInventory.SetWeaponSprite();        
        //Move ItemMenu Into Place
        selectionMenu.transform.position = new Vector3(0, selectionMenu.transform.position.y, selectionMenu.transform.position.z);
        secondaryMenu.transform.position = new Vector3(140, secondaryMenu.transform.position.y, secondaryMenu.transform.position.z);
        itemMenu.transform.position = new Vector3(160, itemMenu.transform.position.y, itemMenu.transform.position.z);
        itemMenu.SetActive(false);
        ItemMenuActive = false;
        //Reset Menu Buttons
        MenuButtonStart();
        
    }
    void OnDisable() {
        characterController.GetComponent<WalkingSFX>().enabled = true;
        SoundManager.PlaySound(SoundManager.Sound.menuExit);
    }

    void Update(){        
        if(ComponentController.inventoryItemsMoving) return;
        if(Input.GetButtonDown("Submit")) MenuSelectionOptions();
        SecondaryMenuActiveController();
        if(SecondaryMenuSelected) return;
        if(ItemMenuActive)UI_Controller();
        if(MenuItemSelected) return;
        MainMenuActiveController();        
    }

    void MenuHighLight(){
        mainMenuItems[mainMenuInt].GetComponent<Image>().sprite = HighLighted[mainMenuInt];
        itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = itemHighLighted[itemMenuInt];
    }

    void MenuUnSelected(){
        mainMenuItems[mainMenuInt].GetComponent<Image>().sprite = unSelected[mainMenuInt];
        itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = itemunSelected[itemMenuInt];
    }

    public void MenuSelected(){    
        if(!MenuItemSelected){mainMenuItems[mainMenuInt].GetComponent<Image>().sprite = Selected[mainMenuInt]; MenuItemSelected = true;} else {MenuHighLight(); MenuItemSelected = false;}  
        if(mainMenuActive[0]) ItemMenuSelected();
        if(mainMenuActive[1]) {SoundManager.PlaySound(SoundManager.Sound.menuEnter); FileMenuSelected();}
        if(mainMenuActive[2]) {SoundManager.PlaySound(SoundManager.Sound.menuEnter); MapMenuSelected();}
        if(mainMenuActive[3]) StartCoroutine(ExitMenu()); //Exit is Selected   
    }
    public void ItemMenuSelecter(){    
        //if(!itemItemSelected){itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = Selected[itemMenuInt];} else {MenuHighLight();}  
        if(itemMenuActive[0]) {uiInventory.WeaponEquip(); Debug.Log("Equip"); ReturnToItemList();}
        if(itemMenuActive[1]) CombinableMenuSelected();
        if(itemMenuActive[2]) CheckItemMenuSlected();  
        if(itemMenuActive[3]) ReturnToItemList();  
    }

    void CombinableMenuSelected(){
        if(!CombineItemActive){
            MenuHighLight();
            Debug.Log("Combine 1");
            uiInventory.SelectCombinableItem();
            SecondaryMenuSelected = false;
            CombineItemActive = true; 
            return;
            }
        if(CombineItemActive){
            // if(uiInventory.returnSelected){
            //         CombineItemActive = false;
            //         Debug.Log("Cant Combine Return");
            //         SecondaryMenuSelected = true;
            //         MenuUnSelected();
            //         return;
            //         }
            Debug.Log("Combine 2");
            SecondaryMenuSelected = true;
            ReturnToItemList(); 
            uiInventory.SelectSecondCombinableItem();
            CombineItemActive = false;
            return;
            }
    }

    void CheckItemMenuSlected(){
            if(!CheckItemActive){
                SoundManager.PlaySound(SoundManager.Sound.menuEnter);
                itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = itemSelected[itemMenuInt];
                Debug.Log("Check"); CheckItemActive = true; uiInventory.CheckItemInfo(); return;}
            
            if(CheckItemActive){
                SoundManager.PlaySound(SoundManager.Sound.menuExit);
                CheckItemActive = false; uiInventory.CheckItemOff();
                itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = itemHighLighted[itemMenuInt]; return;}    
    }

    void ReturnToItemList(){
        //if(SecondaryMenuSelected){
        //    SoundManager.PlaySound(SoundManager.Sound.menuExit);
        //    //Tween.Move(secondaryMenu, new Vector3(160, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f, 
        //    ()=> Tween.Move(selectionMenu, new Vector3(0, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f ,
        //    ()=> ReturningToList()));
            
        //    void ReturningToList(){
        //    itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = itemunSelected[itemMenuInt];
        //    itemMenuActive[itemMenuInt] = false;
        //    itemMenuInt = 0;
        //    SecondaryMenuSelected = false;}
        //}
    }

    void ItemMenuSelected(){
        if(SecondaryMenuSelected) return;
        

        if(uiInventory.returnSelected){
            ComponentController.inventoryItemsMoving= true;
            //Tween.Move(itemMenu, new Vector3(160, itemMenu.transform.position.y, itemMenu.transform.position.z), 0.25f, ()=> itemMenu.SetActive(false));
            //SoundManager.PlaySound(SoundManager.Sound.menuExit);
            ItemMenuActive = false;}


        if(uiInventory.weaponSelected){
            itemSelected[0] = Equip[0];
            itemunSelected[0] = Equip[1];
            itemHighLighted[0] = Equip[2];
            itemMenuItems[0].GetComponent<Image>().sprite = itemHighLighted[0];
            SoundManager.PlaySound(SoundManager.Sound.menuEnter);
            SecondaryMenuSelected = true;
            secondaryMenu.SetActive(true);
            //Tween.Move(selectionMenu, new Vector3(160, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f,
            //            ()=> Tween.Move(secondaryMenu, new Vector3(4, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f, ()=> itemMenuActive[itemMenuInt] = true));
        }                

        if(uiInventory.usableSelected){
            itemSelected[0] = Use[0];
            itemunSelected[0] = Use[1];
            itemHighLighted[0] = Use[2];
            itemMenuItems[0].GetComponent<Image>().sprite = itemHighLighted[0];
            SoundManager.PlaySound(SoundManager.Sound.menuEnter);
            SecondaryMenuSelected = true;
            secondaryMenu.SetActive(true);
            //Tween.Move(selectionMenu, new Vector3(160, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f,
            //            ()=> Tween.Move(secondaryMenu, new Vector3(4, selectionMenu.transform.position.y, selectionMenu.transform.position.z), 0.25f, ()=> itemMenuActive[itemMenuInt] = true));
                        
        }
        if(!itemMenu.activeSelf){
            SoundManager.PlaySound(SoundManager.Sound.menuEnter);
            ComponentController.inventoryItemsMoving = true;
            itemMenu.SetActive(true); 
            //Tween.Move(itemMenu, new Vector3(0, itemMenu.transform.position.y, itemMenu.transform.position.z), 0.25f);
            ItemMenuActive = true;
            }

    }
    
    //Turns File Menu on and Off
    void FileMenuSelected(){
        if(!fileMenu.activeSelf) {fileMenu.SetActive(true); FileMenuActive = true;}        
    }
    
    //Turns Map on and Off
    void MapMenuSelected(){
        if(!mapMenu.activeSelf) {mapMenu.SetActive(true); MapMenuActive = true;}
        else {mapMenu.SetActive(false); MapMenuActive = false;}
    }
    
    //Exit Menu Steps
    IEnumerator ExitMenu(){
        yield return new WaitForSecondsRealtime(1);
        MenuHighLight();
        Inventory.MenuSeletionUp = false;
        Inventory.gameIsPaused = !Inventory.gameIsPaused;
        Inventory.PauseGame();
        playerCamera.SetActive(true);
        PSXCamera.SetActive(true);
        MenuItemSelected = false;
        fullMenu.SetActive(false);
    }
    
    // Controls Left and Right on the Main Menu
    void MainMenuActiveController(){        
        ///////////////////D-PAD////////////////////////
        if((Input.GetKeyDown(KeyCode.D) || DpadController.rightD) && mainMenuInt == mainMenuItems.Length-1) {
            MenuUnSelected(); mainMenuActive[mainMenuInt] = false; mainMenuInt=0 ; MenuHighLight(); mainMenuActive[mainMenuInt] = true; DpadController.rightD = false;  
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;} 
        
        if((Input.GetKeyDown(KeyCode.D) || DpadController.rightD) && mainMenuInt <= mainMenuItems.Length-1) {
            MenuUnSelected(); mainMenuActive[mainMenuInt] = false; mainMenuInt++; MenuHighLight(); mainMenuActive[mainMenuInt] = true; DpadController.rightD = false;
            SoundManager.PlaySound(SoundManager.Sound.menuClick);} 
        
        if((Input.GetKeyDown(KeyCode.A) || DpadController.leftD) && mainMenuInt == 0) {
            MenuUnSelected(); mainMenuActive[mainMenuInt] = false; mainMenuInt = mainMenuItems.Length-1; MenuHighLight(); mainMenuActive[mainMenuInt] = true; DpadController.leftD = false; 
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;}
        
        if((Input.GetKeyDown(KeyCode.A) || DpadController.leftD) && mainMenuInt >= 0) {
            MenuUnSelected(); mainMenuActive[mainMenuInt] = false; mainMenuInt--; MenuHighLight(); mainMenuActive[mainMenuInt] = true; DpadController.leftD = false;
            SoundManager.PlaySound(SoundManager.Sound.menuClick);}
        
    }

    
    void SecondaryMenuActiveController(){
        if(!SecondaryMenuSelected) return;
        if(CheckItemActive) return;
        
        if(Input.GetKeyDown(KeyCode.D) && itemMenuInt == itemMenuItems.Length-1) 
            {MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt=0 ; MenuHighLight(); itemMenuActive[itemMenuInt] = true; 
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;} 
           
        if(Input.GetKeyDown(KeyCode.D) && itemMenuInt <= itemMenuItems.Length-1) 
            {MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt++; MenuHighLight(); itemMenuActive[itemMenuInt] = true;
            SoundManager.PlaySound(SoundManager.Sound.menuClick);} 
            
        if(Input.GetKeyDown(KeyCode.A) && itemMenuInt == 0) 
            {MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt = itemMenuItems.Length-1; MenuHighLight(); itemMenuActive[itemMenuInt] = true; 
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;}
           
        if(Input.GetKeyDown(KeyCode.A) && itemMenuInt >= 0) 
            {MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt--; itemMenuActive[itemMenuInt] = true; MenuHighLight();
            SoundManager.PlaySound(SoundManager.Sound.menuClick);}
            
        
        if(DpadController.rightD && itemMenuInt == itemMenuItems.Length-1) {
            MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt=0 ; MenuHighLight(); itemMenuActive[itemMenuInt] = true; DpadReset(); 
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;} 
        
        if(DpadController.rightD && itemMenuInt <= itemMenuItems.Length-1) {
            MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt++;  MenuHighLight(); itemMenuActive[itemMenuInt] = true; DpadReset();
            SoundManager.PlaySound(SoundManager.Sound.menuClick);} 
        
        if(DpadController.leftD && itemMenuInt == 0) {
            MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt = itemMenuItems.Length-1; MenuHighLight(); itemMenuActive[itemMenuInt] = true; DpadReset();  
            SoundManager.PlaySound(SoundManager.Sound.menuClick);
            return;}
        
        if(DpadController.leftD && itemMenuInt >= 0) {
            MenuUnSelected(); itemMenuActive[itemMenuInt] = false; itemMenuInt--; MenuHighLight(); itemMenuActive[itemMenuInt] = true; DpadReset();
            SoundManager.PlaySound(SoundManager.Sound.menuClick);}
        
    }

    //Resets All Inventory On Open
    void MenuButtonStart(){
        DpadReset();
        MenuItemSelected = false;
        mainMenuItems[3].GetComponent<Image>().sprite = unSelected[3];
        mainMenuActive = new bool [mainMenuItems.Length];
        itemMenuActive = new bool [itemMenuItems.Length];
        itemMenuInt = 0;
        mainMenuInt = 0;
        mainMenuItems[mainMenuInt].GetComponent<Image>().sprite = HighLighted[mainMenuInt];
        itemMenuItems[itemMenuInt].GetComponent<Image>().sprite = HighLighted[itemMenuInt];
        mainMenuActive[mainMenuInt] = true;
        uiInventory.usableSelected = false;
        uiInventory.weaponSelected = false;
        SecondaryMenuSelected = false;
    }

    // Controlls Up and Down Item Menu
    void UI_Controller(){
        if(SecondaryMenuSelected) return;
        if(DpadController.upD && !ComponentController.inventoryItemsMoving) {uiInventory.InventoryItemsUpwards(); SoundManager.PlaySound(SoundManager.Sound.menuClick);}
        if(DpadController.downD && !ComponentController.inventoryItemsMoving) {uiInventory.InventoryItemsDownWards(); SoundManager.PlaySound(SoundManager.Sound.menuClick);}  
    }

    //Quick Dirty Dpad Disabler
    void DpadReset(){
        DpadController.upD    = false;
        DpadController.downD  = false;
        DpadController.leftD  = false;
        DpadController.rightD = false;
    }

    void MenuSelectionOptions(){
       if(ItemMenuActive && !CombineItemActive) ItemMenuSelected();
       if(SecondaryMenuSelected || CombineItemActive)  ItemMenuSelecter();
       if(!FileMenuActive && !ItemMenuActive) MenuSelected(); 
    }
}
