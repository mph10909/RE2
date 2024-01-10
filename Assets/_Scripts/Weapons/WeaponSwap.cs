using System;
using UnityEngine;
using ResidentEvilClone;

[Serializable]
public class WeaponSwap : MonoBehaviour, IControllable, IComponentSavable
{
    [SerializeField] private Item weapon;
    [SerializeField] private Transform weaponLocation;
    private Aiming aiming;

    [SerializeField] WeaponData weaponData;
    private PointAndClick characterController;

    public Item Weapon { get { return weapon; } set { weapon = value; } }

    private void Awake()
    {
        characterController = GetComponent<PointAndClick>();
        aiming = GetComponent<Aiming>();
        //eWeaponSet();
    }
     
    private void OnEnable()
    {
        Actions.SetWeapon += SetWeapon;
        Actions.WeaponToStorage += WeaponToStorage;
    }

    private void OnDisable()
    {
        Actions.SetWeapon -= SetWeapon;
    }

    private void Start()
    {

        if (Loader.Loaded)
            return;

        WeaponSet();
    }

    private void WeaponToStorage(Item storageWeapon)
    {
        if (weapon.itemType == storageWeapon.itemType)
        {
            PreviousWeaponDestroyer();
            weapon.itemType = Item.ItemType.None;
        }
    }

    public void SetWeaponOnLoad(Item weaponSet)
    {
        PreviousWeaponDestroyer();
        weapon.itemType = weaponSet.itemType;
        WeaponSet();
    }

    public void SetWeapon(Item weaponSet)
    {
        if (!this.enabled)
            return;

        InstantiateWeapon(weaponSet);
    }

    private void WeaponSet()
    {
        weaponData = GetWeaponDataFromDatabase(weapon.itemType);
        SetInfo();
    }

    private WeaponData GetWeaponDataFromDatabase(Item.ItemType weaponType) // New method
    {
        foreach (var entry in WeaponsAsset.Instance.weaponDatabase.weapons)
        {
            if (entry.weaponType == weaponType)
            {
                return entry.weaponData;
            }
        }
        return null;
    }

    private void SetInfo()
    {
        if (!this.enabled) return;
        if (weaponData.weaponObject != null)
        {
            GameObject instantiatedWeapon = Instantiate(weaponData.weaponObject, weaponLocation.position, weaponLocation.rotation, weaponLocation);
            aiming.weaponEquipped = instantiatedWeapon.GetComponent<Weapon>();
        }
        else { print("Null"); }

        characterController.PlayerAnimator.SetFloat("Weapon", weaponData.weaponNumber);
        
    }

    private void InstantiateWeapon(Item weaponSet)
    {
        PreviousWeaponDestroyer();

        if (weapon.itemType == weaponSet.itemType)
        {
            weapon.itemType = Item.ItemType.None;
            WeaponSet();
            return;
        }

        weapon.itemType = weaponSet.itemType;
        WeaponSet();
    }

    private void PreviousWeaponDestroyer()
    {
        foreach (Transform child in weaponLocation)
        {
            Destroy(child.gameObject);
        }
    }

    public void EnableControl(bool enable)
    {
        this.enabled = enable;
    }

    public string GetSavableData()
    {
        return weapon.itemType.ToString();
    }

    public void SetFromSaveData(string savedData)
    {
        foreach (Item.ItemType itemType in Enum.GetValues(typeof(Item.ItemType)))
        {
            if (itemType.ToString() == savedData)
            {
                PreviousWeaponDestroyer();
                Weapon.itemType = itemType;
                WeaponSet();
                return;
            }
        }
    }




}
