using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CameraSaveData : MonoBehaviour, ISaveable
    {
        private SaveManager _saveManager;
        public SaveManager SaveManger => _saveManager;

        public void OnCreated(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public void SaveData(SaveData saveData)
        {
            SaveData.CameraData cameraData = new SaveData.CameraData();

            cameraData.name = this.gameObject.name;
            cameraData.rotation = transform.rotation;
            cameraData.position = transform.position;
            cameraData.enabled = this.gameObject.activeSelf;
            saveData.cameraData.Add(cameraData);

        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.CameraData cameraData in saveData.cameraData)
            {
                print("CameraLoaded");
                if(cameraData.name == this.gameObject.name)
                {        
                    transform.position = cameraData.position;
                    transform.rotation = cameraData.rotation;
                    transform.gameObject.SetActive(cameraData.enabled);
                }
            }

        }
    }
}
