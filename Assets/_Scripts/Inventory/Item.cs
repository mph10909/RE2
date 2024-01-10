using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

[Serializable]
public class Item
{
    public enum ItemType
    {
        //Items//
        None,
        Green_Herb,
        Red_Herb,
        Blue_Herb,
        First_Aid_Spray,
        Mixed_Herb_GG,
        Mixed_Herb_GR,
        Mixed_Herb_GB,
        Mixed_Herb_GGB,
        Mixed_Herb_GGG,
        Mixed_Herb_GRB,
        Fire_Extinguisher,
        Ink_Ribbon,
        //Ammo//
        Handgun_Bullets,
        Shotgun_Bullets,
        Grenade_Rounds,
        Acid_Rounds,
        Flame_Rounds,
        Magnum_Rounds,
        //Weapons//
        Handgun_A,
        Handgun_B,
        Handgun_C,
        Handgun_D,
        Shotgun,
        Grenade_Gun,
        Magnum,
        RocketLauncher,
        //Keys//
        Churchs_Rear_Key,
        Rusted_Key,
        Chapel_Key,
        Clock_Winder,
        Film,
        Manhole_Opener,
        Managers_Key,
        Rope,
        Movie_Reel,
        Cracked_Key,
        Aracade_Key,
        Medicine_Room_Key,
        Prison_Cell_Key,
        Club_Hall_Key,
        Card_Key,
        Activation_Disk,
        ID_card,
        Master_Key,
        Knife,
        ColtSAA,
        Theater_Key,
        Restaurant_Storage,
        CrowBar,
    }



public ItemType itemType;
public int amount;

public ItemType GetItemType(){
    return itemType;
}

public ItemType GetAmmo(){
    switch (itemType){
        default:
        case ItemType.Handgun_A:       return ItemType.Handgun_Bullets;
        case ItemType.Shotgun:         return ItemType.Shotgun_Bullets;
        case ItemType.Magnum:          return ItemType.Magnum_Rounds;
        case ItemType.ColtSAA:         return ItemType.Handgun_Bullets;
        }
}

public AmmoType WeaponAmmo()
    {
        switch (itemType)
        {
            default:
            case ItemType.Handgun_A: return AmmoType.Handgun;
            case ItemType.Handgun_Bullets: return AmmoType.Handgun;
            case ItemType.Shotgun: return AmmoType.Shotgun;
            case ItemType.Shotgun_Bullets: return AmmoType.Shotgun;
            case ItemType.Magnum: return AmmoType.Magnum;
            case ItemType.Magnum_Rounds: return AmmoType.Magnum;
            case ItemType.ColtSAA: return AmmoType.Colt;
        }
    }

    public GameObject GetWeapon()
    {
        foreach (WeaponDB.WeaponEntry weaponEntry in WeaponsAsset.Instance.weaponDatabase.weapons)
        {
            if (weaponEntry.weaponType == itemType)
            {
                if (weaponEntry.weaponData.weaponObject != null)
                {
                    return weaponEntry.weaponData.weaponObject;
                }
                else
                {
                    // Return a default weaponObject in case of null (similar to how you return greenHerb in the other method)
                    return null;
                }
            }
        }

        // Return a default weaponObject if the weapon type is not found in the database
        return null;
    }

    public GameObject GetInspectorObject()
    {
        foreach (ItemData item in ItemAssets.Instance.items)
        {
            if (item.item == itemType)
            {
                if (item.itemInventoryGameObject != null)
                {
                    return item.itemInventoryGameObject;
                }
                else return ItemAssets.Instance.greenHerb.itemInventoryGameObject;
            }
        }
        return ItemAssets.Instance.greenHerb.itemInventoryGameObject;
    }

public Sprite GetSprite(){
    switch (itemType){
        default:
        case ItemType.None:             return ItemAssets.Instance.NoneSprite;
        case ItemType.Green_Herb:       return ItemAssets.Instance.greenHerb.itemInventorySprite;
        case ItemType.First_Aid_Spray:  return ItemAssets.Instance.firstAidSpray.itemInventorySprite;
        //case ItemType.Red_Herb:         return ItemAssets.Instance.redHerb.itemInventorySprite;
        case ItemType.Blue_Herb:        return ItemAssets.Instance.blueHerb.itemInventorySprite;
        case ItemType.Mixed_Herb_GR:    return ItemAssets.Instance.mixedHerbGR.itemInventorySprite;
        case ItemType.Mixed_Herb_GG:    return ItemAssets.Instance.mixedHerbGG.itemInventorySprite;
        //case ItemType.Mixed_Herb_GB:    return ItemAssets.Instance.mixedHerbGBSprite;
        case ItemType.Mixed_Herb_GGG:   return ItemAssets.Instance.mixedHerbGGGSprite;
        //case ItemType.Mixed_Herb_GGB:   return ItemAssets.Instance.mixedHerbGGBSprite;
        //case ItemType.Mixed_Herb_GRB:   return ItemAssets.Instance.mixedHerbGRBSprite;
        case ItemType.Ink_Ribbon:       return ItemAssets.Instance.Ink_RibbonSprite;
        case ItemType.CrowBar:          return ItemAssets.Instance.crowBar.itemInventorySprite;

        
        case ItemType.Handgun_Bullets: return ItemAssets.Instance.handgunBullets.itemInventorySprite;
        case ItemType.Shotgun_Bullets: return ItemAssets.Instance.shotgunBulletsSprite;
        case ItemType.Knife:           return ItemAssets.Instance.knife.itemInventorySprite;
        //case ItemType.Acid_Rounds:     return ItemAssets.Instance.acidRoundSprite;
        //case ItemType.Flame_Rounds:    return ItemAssets.Instance.flameRoundsSprite;
        //case ItemType.Magnum_Rounds:   return ItemAssets.Instance.magnumRoundsSprite;
        //Weapons
        case ItemType.Handgun_A:       return ItemAssets.Instance.handGun.itemInventorySprite;
        //case ItemType.Handgun_B:       return ItemAssets.Instance.handgunBSprite;
       // case ItemType.Handgun_C:       return ItemAssets.Instance.handgunCSprite;
        //case ItemType.Handgun_D:       return ItemAssets.Instance.handgunDSprite;
        case ItemType.Shotgun:         return ItemAssets.Instance.shotgunSprite;

        //case ItemType.Grenade_Gun:     return ItemAssets.Instance.grenadeGunSprite;
        //case ItemType.Magnum:          return ItemAssets.Instance.magnumSprite;
        case ItemType.ColtSAA:         return ItemAssets.Instance.coltSprite;
        //case ItemType.RocketLauncher:  return ItemAssets.Instance.rocketLauncherSprite;
        
        //Keys
        case ItemType.Fire_Extinguisher: return ItemAssets.Instance.fireExtinguisher.itemInventorySprite;
        case ItemType.Churchs_Rear_Key:  return ItemAssets.Instance.churchRearKeySprite; 
        case ItemType.Rusted_Key:        return ItemAssets.Instance.rustedKeySprite;
        case ItemType.Chapel_Key:        return ItemAssets.Instance.chapelKey.itemInventorySprite;   
        case ItemType.Clock_Winder:      return ItemAssets.Instance.clockWinder.itemInventorySprite; 
        case ItemType.Film:              return ItemAssets.Instance.filmSprite; 
        case ItemType.Manhole_Opener:    return ItemAssets.Instance.manholeOpenerSprite;
        case ItemType.Managers_Key:      return ItemAssets.Instance.managersKeySprite; 
        case ItemType.Rope:              return ItemAssets.Instance.ropeSprite; 
        case ItemType.Theater_Key:       return ItemAssets.Instance.theaterKey.itemInventorySprite;
        case ItemType.Cracked_Key:       return ItemAssets.Instance.crackedKey.itemInventorySprite;
        case ItemType.Aracade_Key:       return ItemAssets.Instance.arcadeKeySprite;
        case ItemType.Medicine_Room_Key: return ItemAssets.Instance.medicineRoomKeySprite; 
        case ItemType.Prison_Cell_Key:   return ItemAssets.Instance.prisonCellKeySprite; 
        case ItemType.Club_Hall_Key:     return ItemAssets.Instance.clubHallKeySprite;
        case ItemType.Card_Key:          return ItemAssets.Instance.cardKeySprite;  
        case ItemType.Activation_Disk:   return ItemAssets.Instance.activationDiskSprite;
        case ItemType.ID_card:           return ItemAssets.Instance.idCardSprite;
        case ItemType.Master_Key:        return ItemAssets.Instance.masterKeySprite;  
        case ItemType.Movie_Reel:        return ItemAssets.Instance.movieReel.itemInventorySprite;
        case ItemType.Restaurant_Storage:return ItemAssets.Instance.restaurantStorage.itemInventorySprite;


    }
}

public Sprite GetCheckSprite(){
    switch (itemType){
        default:
        case ItemType.Green_Herb:      return ItemAssets.Instance.greenHerb.itemCheckSprite;
        //case ItemType.First_Aid_Spray: return ItemAssets.Instance.firstAidSpray.itemCheckSprite;
        //case ItemType.Red_Herb:        return ItemAssets.Instance.redHerb.itemCheckSprite;
        //case ItemType.Blue_Herb:       return ItemAssets.Instance.blueHerbCheckSprite;
        //case ItemType.Mixed_Herb_GR:   return ItemAssets.Instance.mixedHerbGR.itemCheckSprite;
        //case ItemType.Mixed_Herb_GG:   return ItemAssets.Instance.mixedHerbGGCheckSprite;
        //case ItemType.Mixed_Herb_GB:   return ItemAssets.Instance.mixedHerbGBCheckSprite;
        //case ItemType.Mixed_Herb_GGG:  return ItemAssets.Instance.mixedHerbGGGCheckSprite;
        //case ItemType.Mixed_Herb_GGB:  return ItemAssets.Instance.mixedHerbGGBCheckSprite;
        //case ItemType.Mixed_Herb_GRB:  return ItemAssets.Instance.mixedHerbGRBCheckSprite;

        //case ItemType.Handgun_Bullets: return ItemAssets.Instance.handgunBullets.itemCheckSprite;
        //case ItemType.Shotgun_Bullets: return ItemAssets.Instance.shotgunBulletsCheckSprite;
        //case ItemType.Grenade_Rounds:  return ItemAssets.Instance.grenadeRoundCheckSprite;
        //case ItemType.Acid_Rounds:     return ItemAssets.Instance.acidRoundCheckSprite;
        //case ItemType.Flame_Rounds:    return ItemAssets.Instance.flameRoundsCheckSprite;
        //case ItemType.Magnum_Rounds:   return ItemAssets.Instance.magnumRoundsCheckSprite;

        //case ItemType.Handgun_A:       return ItemAssets.Instance.handgunACheckSprite;
        //case ItemType.Handgun_B:       return ItemAssets.Instance.handgunBCheckSprite;
        //case ItemType.Handgun_C:       return ItemAssets.Instance.handgunCCheckSprite;
        //case ItemType.Handgun_D:       return ItemAssets.Instance.handgunDCheckSprite;
        //case ItemType.Shotgun:         return ItemAssets.Instance.shotgunCheckSprite;
        //case ItemType.Grenade_Gun:     return ItemAssets.Instance.grenadeGunCheckSprite;
        //case ItemType.Magnum:          return ItemAssets.Instance.magnumCheckSprite;
        //case ItemType.RocketLauncher:  return ItemAssets.Instance.rocketLauncherCheckSprite;


    }
}

public static string GetText(ItemType itemType)
    {
        switch (itemType){
        default:
        case ItemType.Green_Herb:      return ItemAssets.Instance.greenHerb.itemNameText;
        case ItemType.First_Aid_Spray: return ItemAssets.Instance.firstAidSpray.itemNameText;
        case ItemType.Red_Herb:        return ItemAssets.Instance.redHerb.itemNameText;
        case ItemType.Blue_Herb:       return ItemAssets.Instance.blueHerb.itemNameText;
        case ItemType.Mixed_Herb_GR:   return ItemAssets.Instance.mixedHerbGR.itemNameText;
        case ItemType.Mixed_Herb_GG:   return ItemAssets.Instance.mixedHerbGG.itemNameText;
        //case ItemType.Mixed_Herb_GB:   return ItemAssets.Instance.mixedHerbGBText;
        case ItemType.Mixed_Herb_GGG:  return ItemAssets.Instance.mixedHerbGGGText;
        //case ItemType.Mixed_Herb_GGB:  return ItemAssets.Instance.mixedHerbGGBText;
        //case ItemType.Mixed_Herb_GRB:  return ItemAssets.Instance.mixedHerbGRBText;
        case ItemType.Ink_Ribbon:      return ItemAssets.Instance.Ink_RibbonText;
        
        case ItemType.Handgun_Bullets: return ItemAssets.Instance.handgunBullets.itemNameText;
        case ItemType.Shotgun_Bullets: return ItemAssets.Instance.shotgunBulletsText;
        //case ItemType.Grenade_Rounds:  return ItemAssets.Instance.grenadeRoundText;
        //case ItemType.Acid_Rounds:     return ItemAssets.Instance.acidRoundText;
        //case ItemType.Flame_Rounds:    return ItemAssets.Instance.flameRoundsText;
        //case ItemType.Magnum_Rounds:   return ItemAssets.Instance.magnumRoundsText;

        case ItemType.Handgun_A:       return ItemAssets.Instance.handGun.itemNameText;
        case ItemType.Knife:           return ItemAssets.Instance.knife.itemNameText;
        //case ItemType.Handgun_C:       return ItemAssets.Instance.handgunCText;
        //case ItemType.Handgun_D:       return ItemAssets.Instance.handgunDText;
        case ItemType.Shotgun:         return ItemAssets.Instance.shotgunText;
        //case ItemType.Grenade_Gun:     return ItemAssets.Instance.grenadeGunText;
        //case ItemType.Magnum:          return ItemAssets.Instance.magnumText;
        case ItemType.ColtSAA:         return ItemAssets.Instance.coltText;
        //case ItemType.RocketLauncher:  return ItemAssets.Instance.rocketLauncherText;

        case ItemType.Fire_Extinguisher: return ItemAssets.Instance.fireExtinguisher.itemNameText; 
        //case ItemType.Churchs_Rear_Key:  return ItemAssets.Instance.churchRearKeyText; 
        //case ItemType.Rusted_Key:        return ItemAssets.Instance.rustedKeyText;
        case ItemType.Chapel_Key:        return ItemAssets.Instance.chapelKey.itemNameText;   
        case ItemType.Clock_Winder:      return ItemAssets.Instance.clockWinder.itemNameText; 
        //case ItemType.Film:              return ItemAssets.Instance.filmText; 
        //case ItemType.Manhole_Opener:    return ItemAssets.Instance.manholeOpenerText;
        //case ItemType.Managers_Key:      return ItemAssets.Instance.managersKeyText; 
        //case ItemType.Rope:              return ItemAssets.Instance.ropeText; 
        case ItemType.Theater_Key:       return ItemAssets.Instance.theaterKey.itemNameText;
        case ItemType.Cracked_Key:       return ItemAssets.Instance.crackedKey.itemNameText;
        //case ItemType.Aracade_Key:       return ItemAssets.Instance.arcadeKeyText;
        //case ItemType.Medicine_Room_Key: return ItemAssets.Instance.medicineRoomKeyText; 
        //case ItemType.Prison_Cell_Key:   return ItemAssets.Instance.prisonCellKeyText; 
        //case ItemType.Club_Hall_Key:     return ItemAssets.Instance.clubHallKeyText;
        //case ItemType.Card_Key:          return ItemAssets.Instance.cardKeyText;  
        //case ItemType.Activation_Disk:   return ItemAssets.Instance.activationDiskText;
        //case ItemType.ID_card:           return ItemAssets.Instance.idCardText;
        //case ItemType.Master_Key:        return ItemAssets.Instance.masterKeyText;
        case ItemType.Movie_Reel:        return ItemAssets.Instance.movieReel.itemNameText;
        case ItemType.Restaurant_Storage:return ItemAssets.Instance.restaurantStorage.itemNameText;
        case ItemType.CrowBar:           return ItemAssets.Instance.crowBar.itemNameText;
        } 
}


public string GetText(){
        return (GetText(this.itemType));
}

public string GetDescriptionText(){

        string noDescription = "No Description Found";

        foreach(ItemData item in ItemAssets.Instance.items)
        {
            if (item.item == itemType)
            {
                return item.itemInventoryDescription;
            }
            
        }

        return noDescription;
}

public bool IsStackable(){
    switch (itemType){
    case ItemType.Handgun_Bullets:
    case ItemType.Shotgun_Bullets:
    case ItemType.Grenade_Rounds:
    case ItemType.Acid_Rounds:
    case ItemType.Flame_Rounds:
    case ItemType.Magnum_Rounds:
    case ItemType.Ink_Ribbon:
        return true;
    default:
        return false;
    }
}

public bool IsEquipable(){
    switch (itemType){
    
    case ItemType.Handgun_A:  
    case ItemType.Handgun_B:
    case ItemType.Handgun_C:
    case ItemType.Handgun_D:
    case ItemType.Shotgun:
    case ItemType.Grenade_Gun:     
    case ItemType.Magnum:          
    case ItemType.RocketLauncher:
    case ItemType.Handgun_Bullets:
    case ItemType.Shotgun_Bullets:
    case ItemType.Grenade_Rounds:
    case ItemType.Acid_Rounds:
    case ItemType.Flame_Rounds:
    case ItemType.Magnum_Rounds:
    case ItemType.ColtSAA:
    case ItemType.Knife:
                return true;

    default:
                return false;
    }
}

public bool IsIncreasable()
{
    switch (itemType)
    {
        default:
        case ItemType.First_Aid_Spray:
            return true;
    }
}

public bool IsWeapon(){
    switch (itemType){
    case ItemType.Handgun_A:  
    case ItemType.Handgun_B:
    case ItemType.Handgun_C:
    case ItemType.Handgun_D:
    case ItemType.Shotgun:
    case ItemType.Grenade_Gun:     
    case ItemType.Magnum:          
    case ItemType.RocketLauncher:
    case ItemType.ColtSAA:
    case ItemType.Knife:
        return true;
    
    default:   
        return false;
    }
}

public bool IsKey(){
    switch (itemType){
    default:
    case ItemType.Churchs_Rear_Key: 
    case ItemType.Rusted_Key:
    case ItemType.Chapel_Key:        
    case ItemType.Clock_Winder:      
    case ItemType.Film:              
    case ItemType.Manhole_Opener:
    case ItemType.Managers_Key:    
    case ItemType.Rope:             
    case ItemType.Theater_Key:       
    case ItemType.Cracked_Key:       
    case ItemType.Aracade_Key:       
    case ItemType.Medicine_Room_Key: 
    case ItemType.Prison_Cell_Key:   
    case ItemType.Club_Hall_Key:     
    case ItemType.Card_Key:          
    case ItemType.Activation_Disk:   
    case ItemType.ID_card:           
    case ItemType.Master_Key:
        return true;

    case ItemType.None:
    case ItemType.Green_Herb:
    case ItemType.Red_Herb:
    case ItemType.Blue_Herb:
    case ItemType.First_Aid_Spray:
    case ItemType.Mixed_Herb_GG:
    case ItemType.Mixed_Herb_GR:
    case ItemType.Mixed_Herb_GB:
    case ItemType.Mixed_Herb_GGB:
    case ItemType.Mixed_Herb_GGG:
    case ItemType.Mixed_Herb_GRB:

    case ItemType.Handgun_A:  
    case ItemType.Handgun_B:
    case ItemType.Handgun_C:
    case ItemType.Handgun_D:
    case ItemType.Shotgun:
    case ItemType.Grenade_Gun:     
    case ItemType.Magnum:          
    case ItemType.RocketLauncher:

    case ItemType.Handgun_Bullets:
    case ItemType.Shotgun_Bullets:
    case ItemType.Grenade_Rounds:
    case ItemType.Acid_Rounds:
    case ItemType.Flame_Rounds:
    case ItemType.Magnum_Rounds:
        return false;
    }
}

public bool IsUsable(){
    switch (itemType){
    case ItemType.Handgun_A:  
    case ItemType.Handgun_B:
    case ItemType.Handgun_C:
    case ItemType.Handgun_D:
    case ItemType.Shotgun:
    case ItemType.Grenade_Gun:     
    case ItemType.Magnum:          
    case ItemType.RocketLauncher:
    case ItemType.Knife:

    case ItemType.Handgun_Bullets:
    case ItemType.Shotgun_Bullets:
    case ItemType.Grenade_Rounds:
    case ItemType.Acid_Rounds:
    case ItemType.Flame_Rounds:
    case ItemType.Magnum_Rounds:
        return false;

    default:
        return true;
        }
    }

public bool IsAmmo()
    {
        switch (itemType)
        {
            default:
            case ItemType.Handgun_Bullets:
            case ItemType.Shotgun_Bullets:
            case ItemType.Grenade_Rounds:
            case ItemType.Acid_Rounds:
            case ItemType.Flame_Rounds:
            case ItemType.Magnum_Rounds:
                return true;
        }
    }
}
