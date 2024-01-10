using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class SaveDataManagerButton : MonoBehaviour
    {
        string Test;

        public void SaveData()
        {
            SaveDataManager.Instance.SaveSlot(1 ,Test);
        }

        public void LoadData()
        {
            SaveDataManager.Instance.LoadSlot();
        }

        [ContextMenu("Save Data Via ContextMenu")]
        private void SaveDataFromMenu()
        {
            SaveData();
        }

        [ContextMenu("Save Data Via ContextMenu")]
        private void LoadDataFromMenu()
        {
            print("Load");
            LoadData();
        }
    }
}
