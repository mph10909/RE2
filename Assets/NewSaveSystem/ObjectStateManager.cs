using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResidentEvilClone
{
    [Serializable]
    public struct ObjectStateSavable
    {
        public List<SaveDataEntry> ObjectState;
    }

    public class ObjectStateManager : Singleton<ObjectStateManager>, ISavable<ObjectStateSavable>
    {
        Dictionary<string, SaveDataEntry> States = new Dictionary<string, SaveDataEntry>();

        public bool IsSceneTransition { get; set; } = false;

        public void CaptureStates()
        {
            SavableObject[] objects = UnityEngine.Object.FindObjectsOfType<SavableObject>(true);
            foreach (SavableObject obj in objects)
            {
                //Debug.Log($"Captured States Count: {States.Count}");
                //Debug.Log($"Found SavableObject: {obj.name}");
                if (obj.enabled)
                {
                    SetState(obj.GetSavableData());
                }
            }
        }

        public void ApplyStates()
        {
            SavableObject[] objects = UnityEngine.Object.FindObjectsOfType<SavableObject>(true);
            foreach (SavableObject obj in objects)
            {
                // Skip applying state to the player during scene transition
                if (IsSceneTransition && obj.CompareTag("Player"))
                    continue;

                ID id = obj.GetComponent<ID>();
                if (GetState(id.UniqueID.ToString(), out SaveDataEntry data))
                {
                    obj.SetFromSaveData(data);
                }
                else
                {
                    // Print additional information about the object
                    Debug.Log($"No state found for ID: {id.UniqueID.ToString()}");
                    Debug.Log($"Object Name: {obj.name}"); // Print the object's name
                    Debug.Log($"Object Tag: {obj.tag}");   // Print the object's tag or any other relevant data
                }
            }

            // Reset the flag after applying states
            IsSceneTransition = false;
        }

        public bool GetState(string id, out SaveDataEntry data)
        {
            if (!States.ContainsKey(id))
            {
                data = new SaveDataEntry();
                return false;
            }
            else
            {
                data = States[id];
                return true;
            }
        }

        public void SetFromSaveData(ObjectStateSavable savedData)
        {
            States.Clear();

            foreach(SaveDataEntry entry in savedData.ObjectState)
            {
                //Debug.Log(entry.Name);
                States.Add(entry.UniqueID.ToString(), entry);
            }
        }

        private void SetState(SaveDataEntry saveDataEntry)
        {
            States[saveDataEntry.UniqueID.ToString()] = saveDataEntry;
        }

        public ObjectStateSavable GetSavableData()
        {
            List<SaveDataEntry> objects = new List<SaveDataEntry>();
            foreach (KeyValuePair<string, SaveDataEntry> data in States)
            {
                objects.Add(data.Value);
            }

            ObjectStateSavable savableData = new ObjectStateSavable()
            {
                ObjectState = objects
            };

            Debug.Log($"Objects to Serialize Count: {savableData.ObjectState.Count}");
            string jsonString = JsonUtility.ToJson(savableData);
            //Debug.Log($"Serialized JSON: {jsonString}");

            return savableData;
        }

    }
}
