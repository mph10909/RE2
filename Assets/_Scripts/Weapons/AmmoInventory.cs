using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public enum AmmoType : int
    {
        None,
        Handgun,
        Shotgun,
        Bow,
        Grenade,
        AcidGrenade,
        FlameGrenade,
        Magnum,
        MachineGun,
        Colt
    }

    public class AmmoInventory : MonoBehaviour, IComponentSavable
    {
        [SerializeField] PlayerInventory itemInventory;

        public List<AmmoEntry> _inventory = new List<AmmoEntry>();

        [System.Serializable]
        public struct AmmoEntry
        {

//#if UNITY_EDITOR
            [HideInInspector]public string name;
//#endif
            public int maxCapacity;
            public Item.ItemType weaponType;
            public Item inventoryAmmo;
            public int loadedCapacity;
            public int loaded;
        }

        void OnEnable()
        {
            SetInventoryAmmo(); 
            Actions.InventoryItemChange += SetInventoryAmmo;
        }

        void OnDisable()
        {
            Actions.InventoryItemChange -= SetInventoryAmmo;
        }

        public int GetStock(AmmoType type)
        {
            return _inventory[(int)type].inventoryAmmo.amount;
        }

        public int Spend(AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int spend = Mathf.Min(amount, held.loaded);
            held.loaded -= spend;
            _inventory[(int)type] = held;
            return spend;
        }

        public int GetEmpty(AmmoType type)
        {
            return _inventory[(int)type].loaded;
        }

        public int Reload(AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int reload = Mathf.Min(amount, held.inventoryAmmo.amount);
            held.inventoryAmmo.amount -= reload;
            itemInventory.inventory.RemoveItem(new Item {
                itemType = held.inventoryAmmo.itemType, amount = reload });         
            held.loaded += reload;
            _inventory[(int)type] = held;
            return reload;
        }

        public int NotEmptyReload(AmmoType type)
        {
            AmmoEntry held = _inventory[(int)type];
            int reload = Mathf.Min(held.loadedCapacity - held.loaded, held.inventoryAmmo.amount);
            held.inventoryAmmo.amount -= reload;
            itemInventory.inventory.RemoveItem(new Item
            {
                itemType = held.inventoryAmmo.itemType,
                amount = reload
            });
            held.loaded += reload;  
            _inventory[(int)type] = held;
            return reload;
        }

        public void SetInventoryAmmo()
        {
            foreach (AmmoEntry weapon in _inventory)
            {
                foreach (Item item in itemInventory.inventory.GetItemList())
                {
                    if (weapon.inventoryAmmo.itemType == item.itemType)
                    {
                        weapon.inventoryAmmo.amount = item.amount;
                    }
                }
            }
        }

        public void AmmoToStorage(Item ammo)
        {
            foreach (AmmoEntry weapon in _inventory)
            {
                if(weapon.inventoryAmmo.itemType == ammo.itemType)
                {
                    weapon.inventoryAmmo.amount = 0;
                }
            }
        }

        public void FillWeaponOnLoad(List<int> savedAmmo, List<int> stockedAmmo)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                _inventory[i] = new AmmoEntry
                {
                    maxCapacity = _inventory[i].maxCapacity,
                    name = _inventory[i].name,
                    weaponType = _inventory[i].weaponType,
                    inventoryAmmo = new Item {itemType = _inventory[i].inventoryAmmo.itemType, amount = stockedAmmo[i] },
                    loadedCapacity = _inventory[i].loadedCapacity,
                    loaded = savedAmmo[i]
                };
            }
        }

        public List<int> AmmoLoaded()
        {
            List<int> loaded = new List<int>();
            foreach (AmmoInventory.AmmoEntry ammo in _inventory)
            {
                loaded.Add(ammo.loaded);
            }
            return loaded;

        }

        public List<int> AmmoStocked()
        {
            List<int> loaded = new List<int>();
            foreach (AmmoInventory.AmmoEntry ammo in _inventory)
            {
                loaded.Add(ammo.inventoryAmmo.amount);
            }
            return loaded;
        }

#if UNITY_EDITOR
        void Reset() { OnValidate(); }
        void OnValidate()
        {
            var ammoNames = System.Enum.GetNames(typeof(AmmoType));
            var inventory = new List<AmmoEntry>(ammoNames.Length);
            for (int i = 0; i < ammoNames.Length; i++)
            {
                var existing = _inventory.Find(
                    (entry) => { return entry.name == ammoNames[i]; });
                existing.name = ammoNames[i];
                inventory.Add(existing);
            }
            _inventory = inventory;
        }

        public string GetSavableData()
        {
            List<string> dataList = new List<string>();

            List<int> loaded = AmmoLoaded();
            List<int> stocked = AmmoStocked();

            for (int i = 0; i < loaded.Count; i++)
            {
                dataList.Add($"{loaded[i]};{stocked[i]}");
            }

            return string.Join("|", dataList);
        }

        public void SetFromSaveData(string savedData)
        {
            string[] entryDataList = savedData.Split('|');

            List<int> savedAmmo = new List<int>();
            List<int> stockedAmmo = new List<int>();

            for (int i = 0; i < entryDataList.Length; i++)
            {
                string[] fields = entryDataList[i].Split(';');
                if (fields.Length != 2) // We expect two fields for each entry
                {
                    Debug.LogError($"Error in parsing savedData at entry {i}");
                    continue;
                }

                int loaded = int.Parse(fields[0]);
                int amount = int.Parse(fields[1]);

                savedAmmo.Add(loaded);
                stockedAmmo.Add(amount);
            }

            FillWeaponOnLoad(savedAmmo, stockedAmmo);
        }



#endif

    }
}
