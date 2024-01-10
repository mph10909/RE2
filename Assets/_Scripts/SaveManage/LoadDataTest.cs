using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class LoadDataTest : MonoBehaviour
    {
        [SerializeField] int saveAmount;
        [SerializeField] string loadData;
        [SerializeField] Text saveInfo;
        SaveData data;

        void OnEnable()
        {
            PopulateSave();
        }

        public void PopulateSave()
        {
            data = SaveManager.GetData("Save" + saveAmount);
            if (data.activePlayer == null) return;
            loadData = (saveAmount.ToString() + "_:_" + data.activePlayer + "_/_" + data.saveAmount.ToString() + "_/_" + data.roomName);
            saveInfo.text = loadData;
        }

    }
}
