using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class StorageSaveData : MonoBehaviour, ISaveable
    {
        public void SaveData(SaveData saveData)
        {
            SaveData.StorageData storageData = new SaveData.StorageData();

            storageData.name = this.name;
            storageData.item = GetComponent<StorageButton>().storageItem;


            saveData.storageData.Add(storageData);

        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.StorageData storageData in saveData.storageData)
            {
                if (storageData.name == this.gameObject.name)
                {
                    GetComponent<StorageButton>().storageItem = storageData.item;
                    break;
                }
            }

        }
    }
}
