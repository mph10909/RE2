using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public enum InventoryCombinations
    {
        Handgun,
        Shotgun,
        Colt,
        //Bow,
        //Grenade,
        //AcidGrenade,
        //FlameGrenade,
        //Magnum,
        //MachineGun,

        Mixed_Herb_GG,
        Mixed_Herb_GR,
        Mixed_Herb_GB,
        Mixed_Herb_GGB_1,
        Mixed_Herb_GGB_2,
        Mixed_Herb_GGG,
        Mixed_Herb_GRB_1,
        Mixed_Herb_GRB_2,

        UpdgradedShotgun
    }
    public class CombineItemsManager : MonoBehaviour
    {
        [SerializeField] PncInventory currentCharacter;

        void OnEnable()
        {
            Actions.CombineItems += CheckReceipe;
        }

        void OnDisable()
        {
            Actions.CombineItems -= CheckReceipe;
        }

        public List<Combinables> recipes = new List<Combinables>();

        [System.Serializable]
        public struct Combinables
        {
            #if UNITY_EDITOR
            [HideInInspector]public string name;
            #endif
            public Item.ItemType firstItem;
            public Item.ItemType secondItem;
            public Item combineResult;
            public bool reloadable;
            public bool combinable;
        }

        void CheckReceipe(Item item1, Item item2)
        {
            foreach(Combinables item in recipes)
            {
                if ((item1.itemType == item.firstItem && item2.itemType == item.secondItem) ||
                    (item1.itemType == item.secondItem && item2.itemType == item.firstItem))
                {
                    print("SomeHowThisWorks");
                    SoundManagement.Instance.MenuSound(MenuSounds.Accept);
                    if (item.reloadable) Reloadable(item1);
                    if (item.combinable) Combinable(item1, item2, item.combineResult);
                    return;
                }
            }
            SoundManagement.Instance.MenuSound(MenuSounds.Decline);
        }

        void Reloadable(Item ammo)
        {
            currentCharacter.ammoInventory.NotEmptyReload(ammo.WeaponAmmo());
            RemoveEmptyItems();
        }

        void Combinable(Item firstItem, Item secondItem, Item Result)
        {
            currentCharacter.inventory.inventory.RemoveItem(firstItem);
            currentCharacter.inventory.inventory.RemoveItem(secondItem);
            currentCharacter.inventory.inventory.AddItem(new Item { itemType = Result.itemType, amount = 1 });
            Actions.RefreshInventory?.Invoke();
        }

        void RemoveEmptyItems()
        {
            foreach (Item item in currentCharacter.inventory.inventory.GetItemList())
            {
                if (item.IsStackable())
                {
                    if (item.amount == 0) currentCharacter.inventory.inventory.RemoveItem(item);
                    Actions.RefreshInventory?.Invoke();
                    return;
                }
            }
            
        }

#if UNITY_EDITOR
        void Reset() { OnValidate(); }
        void OnValidate()
        {
            var comboNames = System.Enum.GetNames(typeof(InventoryCombinations));
            var inventory = new List<Combinables>(comboNames.Length);
            for (int i = 0; i < comboNames.Length; i++)
            {
                var existing = recipes.Find(
                    (entry) => { return entry.name == comboNames[i]; });
                existing.name = comboNames[i];
                inventory.Add(existing);
            }
            recipes = inventory;
        }
#endif
    }
}
