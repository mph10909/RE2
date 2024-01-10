using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ResidentEvilClone
{
    public class SaveDataManager : Singleton<SaveDataManager>
    {
        private static readonly string k_SaveFileName = "saveGameSlot";
        private static readonly string k_SaveFileExt = ".dat";

        private int m_CurrentSlot = -1;
        private SaveData m_SaveData;

        public struct SaveData
        {
            public string GameData;
            public ObjectStateSavable ObjectState;
        }

        public void SaveSlot(int slot, string data)
        {
            m_SaveData.GameData = data;
            ObjectStateManager.Instance.CaptureStates();
            m_SaveData.ObjectState = ObjectStateManager.Instance.GetSavableData();

            string saveDataJson = JsonUtility.ToJson(m_SaveData, true);
            byte[] saveDataByteArray = Encoding.ASCII.GetBytes(saveDataJson);

            string savePath = Path.Combine(Application.persistentDataPath, k_SaveFileName + k_SaveFileExt);
            File.WriteAllBytes(savePath, saveDataByteArray);

            string saveTextPath = Path.Combine(Application.persistentDataPath, slot + ".txt");
            try
            {
                File.WriteAllText(saveTextPath, saveDataJson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error writing to file: {e.Message}");
            }

            Debug.Log($"Save data written to: {savePath}");
            Debug.Log($"Text save data written to: {saveTextPath}");
        }

        public void LoadSlot()
        {
            m_SaveData = GetSaveData();

            ObjectStateManager.Instance.SetFromSaveData(m_SaveData.ObjectState);
            ObjectStateManager.Instance.ApplyStates();
        }

        public SaveData GetSaveData()
        {
            string savePath = Path.Combine(Application.persistentDataPath, k_SaveFileName + k_SaveFileExt);

            // Check if the file exists before attempting to read from it.
            if (!File.Exists(savePath))
            {
                Debug.LogError($"Save file not found at: {savePath}");
                return default(SaveData); // Return an empty SaveData object or handle as necessary.
            }
            byte[] data = File.ReadAllBytes(savePath);

            string saveDataJson = Encoding.ASCII.GetString(data);
            return JsonUtility.FromJson<SaveData>(saveDataJson);
        }
    }


    


}
