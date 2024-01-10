using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAssets : MonoBehaviour
{

    public static ItemAssets Instance {get; private set;}

    private void Awake(){
        Instance = this;
    }

    public ItemData[] items;

    public Sprite NoneSprite;

    [Header("Return")]
    [Space(1)][Header("Return")]
    public string returnText;
    public Sprite returnSprite;
    public Sprite returnCheckSprite;
    [TextArea]
    public string returnDescriptionText;
    
    
    [Header("Items")]
    [Space(1)][Header("First Aid Spray")]
    public ItemData firstAidSpray;
   
    [Space(1)][Header("Green Herb")]
    public ItemData greenHerb;


    [Space(1)][Header("Red Herb")]
    public ItemData redHerb;


    [Space(1)][Header("Blue Herb")]
    public ItemData blueHerb;

    [Space(1)]    [Header("Mix Herb Green Red")]
    public ItemData mixedHerbGR;

    [Space(1)][Header("Mix Herb Green Green")]
    public ItemData mixedHerbGG;

    [Space(1)][Header("Mix Herb Green Blue")]
    public string mixedHerbGBText;
    public Sprite mixedHerbGBSprite;
    public Sprite mixedHerbGBCheckSprite;
    [TextArea]
    public string mixedHerbGBDescriptionText;
    public GameObject mixedHerbGBInspectorObject;

    [Space(1)][Header("Mix Herb Green Green Green")]
    public string mixedHerbGGGText;
    public Sprite mixedHerbGGGSprite;
    public Sprite mixedHerbGGGCheckSprite;
    [TextArea]
    public string mixedHerbGGGDescriptionText;

    [Space(1)][Header("Mix Herb Green Green Blue")]
    public string mixedHerbGGBText;
    public Sprite mixedHerbGGBSprite;
    public Sprite mixedHerbGGBCheckSprite;
    [TextArea]
    public string mixedHerbGGBDescriptionText;

    [Space(1)][Header("Mix Herb Green Red Blue")]
    public string mixedHerbGRBText;
    public Sprite mixedHerbGRBSprite;
    public Sprite mixedHerbGRBCheckSprite;
    [TextArea]
    public string mixedHerbGRBDescriptionText;

    [Space(1)][Header("Ink Ribbon")]
    public string Ink_RibbonText;
    public Sprite Ink_RibbonSprite;
    public Sprite Ink_RibbonCheckSprite;
    [TextArea]
    public string Ink_RibbonDescriptionText;
    public GameObject Ink_RibbonGameObject;


    [Space(10)]
    [Header("Ammo")]

    [Space(1)]
    [Header("Handgun Bullets")]

    public ItemData handgunBullets;
    
    
    [Space(1)][Header("Shotgun Bullets")]
    public string shotgunBulletsText;
    public Sprite shotgunBulletsSprite;
    public Sprite shotgunBulletsCheckSprite;
    [TextArea]
    public string shotgunBulletsDescriptionText;
    
    
    [Space(1)][Header("Grenade Rounds")]
    public ItemData grenadeRound;
    
    
    [Space(1)][Header("Acid Rounds")]
    public ItemData acidRound;
    
    
    [Space(1)][Header("Flame Rounds")]
    public ItemData flameRound;
    
    
    [Space(1)][Header("Magnum Rounds")]
    public ItemData magnumRounds;


    [Space(10)][Header("Weapons")]
    
    [Space(1)][Header("Handgun A")]
    public ItemData handGun;

    [Space(1)][Header("Knife")]
    public ItemData knife;  
 
    [Space(1)][Header("Shotgun")]
    public string shotgunText;
    public Sprite shotgunSprite;
    public Sprite shotgunCheckSprite;
    [TextArea]
    public string shotgunDescriptionText;
    public GameObject shotgunGameObject;

    [Space(1)][Header("GrenadeGun")]
    public ItemData grenadeGun;

    [Space(1)][Header("Magnum")]
    public ItemData magnum;

    [Space(1)]
    [Header("Colt")]
    public string coltText;
    public Sprite coltSprite;
    public Sprite coltCheckSprite;
    [TextArea]
    public string coltDescriptionText;
    public GameObject coltGameObject;
    
    [Space(10)][Header("Keys")]

    [Space(1)][Header("FireExtinguisher")]
    public ItemData fireExtinguisher;
    
    
    [Space(1)][Header("Church Rear Key")]
    public string churchRearKeyText; 
    public Sprite churchRearKeySprite; 
    
    [Space(1)][Header("Rusted Key")]
    public string rustedKeyText;
    public Sprite rustedKeySprite;
    public string rustedKeyDescriptionText;
    public GameObject rustedKeyGameObject;

    [Space(1)]
    [Header("Chapel Key")]
    public ItemData chapelKey;

    [Space(1)]
    [Header("Winder Key")]
    public ItemData clockWinder;

    [Space(1)][Header("Film Reel")]
    public string filmText;
    public Sprite filmSprite;              
    
    [Space(1)]
    public string manholeOpenerText;
    public Sprite manholeOpenerSprite;
    
    [Space(1)]
    public string managersKeyText;
    public Sprite managersKeySprite;    
    
    [Space(1)]
    public string ropeText;
    public Sprite ropeSprite;

    [Space(1)]
    [Header("Theater Key")]
    public ItemData theaterKey;

    [Space(1)]
    [Header("Cracked Key")]
    public ItemData crackedKey;

    [Space(1)]
    [Header("Crow Bar")]
    public ItemData crowBar;

    [Space(1)]
    [Header("Arcade Key")]
    public string arcadeKeyText;
    public Sprite arcadeKeySprite;       
   
    [Space(1)]
    public string medicineRoomKeyText;
    public Sprite medicineRoomKeySprite; 
    
    [Space(1)]
    public string prisonCellKeyText;
    public Sprite prisonCellKeySprite;   
    
    [Space(1)]
    public string clubHallKeyText;
    public Sprite clubHallKeySprite;     
    
    [Space(1)]
    public string cardKeyText;
    public Sprite cardKeySprite;          
    
    [Space(1)]
    public string activationDiskText;
    public Sprite activationDiskSprite;   
    
    [Space(1)]
    public string idCardText;
    public Sprite idCardSprite;           
    
    [Space(1)]
    public string masterKeyText;
    public Sprite masterKeySprite;

    [Space(1)][Header("Movie Reel")]
    public ItemData movieReel;

    [Space(1)][Header("R. Storage Key")]
    public ItemData restaurantStorage;



    [Space(4)][Header("Character Sprites")]
    public Sprite leonSprite;
    public Sprite claireSprite;

    [Space(4)][Header("Health")]
    public Sprite fine;
    public Sprite caution;
    public Sprite danger;
    public Sprite poision;
    
}
