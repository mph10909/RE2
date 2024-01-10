using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryCombinations
    {
        LargeHerb,
    }

public class TutorialCombineItemsManager : MonoBehaviour
{

    public List<Combinables> recipes = new List<Combinables>();

    [System.Serializable]
    public struct Combinables
        {
            #if UNITY_EDITOR
            [HideInInspector]public string name;
            #endif
            public TutorialItem.TutItem firstItem;
            public TutorialItem.TutItem secondItem;
            public TutorialItem combineResult;
        }

    public TutorialItem ReturnNewItem(TutorialItem item1, TutorialItem item2)
    {
        foreach (Combinables item in recipes)
        {
            if (item1.item == item.firstItem && item2.item == item.secondItem)      
            {
                return item.combineResult;
            }
            else if (item1.item == item.secondItem && item2.item == item.firstItem)
            {
                return item.combineResult;
            }
        }
        throw new Exception("Combination not found");
    }

    public bool ReturnCombinable(TutorialItem item1, TutorialItem item2)
        {
            foreach (Combinables item in recipes)
            {
                if (item1.item == item.firstItem && item2.item == item.secondItem)
                {
                    return true;
                }
                else if((item1.item == item.secondItem && item2.item == item.firstItem))
                {
                    return true;
                }
            }
            return false;
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
