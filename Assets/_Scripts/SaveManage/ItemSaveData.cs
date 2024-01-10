using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ItemSaveData : ID, ISaveable
    {

        public void SaveData(SaveData saveData)
        {
            SaveData.ItemData itemData = new SaveData.ItemData();

            itemData.name = UniqueID.ToString();
            itemData.active = this.gameObject.activeSelf;

            saveData.itemData.Add(itemData);

        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.ItemData itemData in saveData.itemData)
            {
                if (itemData.name == UniqueID.ToString())
                {
                    this.gameObject.SetActive(itemData.active);
                    break;
                }
            }

        }
    }
}
