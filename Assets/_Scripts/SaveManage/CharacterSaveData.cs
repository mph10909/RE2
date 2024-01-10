using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{

    public class CharacterSaveData : MonoBehaviour, ISaveable
    {
        [SerializeField] AudioSource audioVol;
        [SerializeField] NavMeshAgent nav;
        [SerializeField] PlayerAttacked health;
        [SerializeField] PlayerInventory inventory;
        [SerializeField] AmmoInventory ammo;
        [SerializeField] PointAndClick moveable;
        [SerializeField] Aiming aim;
        [SerializeField] WeaponSwap weapon;
        

        public void OnCreated()
        {
            nav.enabled = true;
        }

        public void LateStart()
        {
            nav.enabled = true;
        }


        public List<int> AmmoLoaded()
        {
            List<int> loaded = new List<int>();
            foreach (AmmoInventory.AmmoEntry ammo in ammo._inventory)
            {
                loaded.Add(ammo.loaded);
            }
            return loaded;
            
        }

        public List<int> AmmoStocked()
        {
            List<int> loaded = new List<int>();
            foreach (AmmoInventory.AmmoEntry ammo in ammo._inventory)
            {
                loaded.Add(ammo.inventoryAmmo.amount);
            }
            return loaded;
        }

        public List<Item> Items()
        {
            List<Item> items = new List<Item>();
            foreach (Item item in inventory.inventory.itemList)
            {
                items.Add(item);
            }
            return items;
        }

        public void SaveData(SaveData saveData)
        {
            SaveData.CharacterData characterData = new SaveData.CharacterData();

            if (moveable.enabled)
            {
                saveData.activePlayer = this.gameObject.name;
            }

            characterData.playerName = this.gameObject.name;
            characterData.playerHealth = health.Health;

            characterData.active = moveable.enabled;
            characterData.aim = aim.enabled;
            characterData.weapon = weapon.enabled;

            characterData.weaponEquipped = weapon.Weapon;

            characterData.audioLevel = audioVol.volume;
            

            characterData.position = transform.position;
            characterData.rotation = transform.rotation;

            characterData.inventory = Items();

            characterData.loadedAmmo = AmmoLoaded();
            characterData.stockedAmmo = AmmoStocked();

            saveData.characterData.Add(characterData);
        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.CharacterData characterData in saveData.characterData)
            {
                print("Charater Loaded");
                if (characterData.playerName == this.gameObject.name)
                {
                    //transform.position = characterData.position;
                    nav.Warp(characterData.position);
                    transform.rotation = characterData.rotation;

                    moveable.enabled = characterData.active;
                    aim.enabled = characterData.aim;
                    weapon.enabled = characterData.weapon;

                    audioVol.volume = characterData.audioLevel;

                    health.Health = characterData.playerHealth;

                    inventory.inventory.itemList = characterData.inventory;

                    Item weaponToEquip = characterData.weaponEquipped;
                    weapon.SetWeaponOnLoad(weaponToEquip);

                    ammo.FillWeaponOnLoad(characterData.loadedAmmo, characterData.stockedAmmo);
                    
                    break;
                }
            }
        }
    }
}
