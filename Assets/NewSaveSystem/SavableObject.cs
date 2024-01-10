using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public interface IComponentSavable : ISavable<string> { }

    [Serializable]
    public struct SaveDataEntry
    {
        public string Name;
        public string UniqueID;
        public Vector3 LocalPosition;
        public Vector3 LocalScale;
        public Vector3 LocalRotation;
        public bool Active;

        public string[] SerializedComponentsTypes;
        public string[] SerializedComponents;

        public string GetComponentData<T>() where T : MonoBehaviour
        {
            string[] serializedComponentsTypes = SerializedComponentsTypes;
            string[] serializedComponents = SerializedComponents;
            for (int i = 0; i < serializedComponentsTypes.Length; ++i)
            {
                string typeName = typeof(T).Name;
                if (serializedComponentsTypes[i] == typeName)
                {
                    return serializedComponents[i];
                }
            }

            return "";
        }
    }

    [RequireComponent(typeof(ID))]
    public class SavableObject : MonoBehaviour, ISavable<SaveDataEntry>
    {
        [SerializeField]
        ID objectID;

        [ContextMenu("Apply Saved State")]
        private void ApplySavedState()
        {
            if (!objectID)
                objectID = GetComponent<ID>();

            if (ObjectStateManager.Instance.GetState(objectID.UniqueID.ToString(), out SaveDataEntry data))
                SetFromSaveData(data);
        }

        public void Awake()
        {
            objectID = GetComponent<ID>();
        }

        public void SetFromSaveData(SaveDataEntry savedData)
        {
            if (!objectID)
                objectID = GetComponent<ID>();

            Debug.Assert(savedData.UniqueID == objectID.UniqueID.ToString(), "No Match");

            transform.localPosition = savedData.LocalPosition;
            transform.localScale = savedData.LocalScale;
            transform.localRotation = Quaternion.Euler(savedData.LocalRotation);
            gameObject.SetActive(savedData.Active);

            string[] serializedComponentsTypes = savedData.SerializedComponentsTypes;
            string[] serializedComponents = savedData.SerializedComponents;
            for (int i = 0; i < serializedComponentsTypes.Length; ++i)
            {
                ISavable<string> c = GetComponent(serializedComponentsTypes[i]) as ISavable<string>;
                if (c != null)
                {
                    c.SetFromSaveData(serializedComponents[i]);
                }
            }

        }

        public SaveDataEntry GetSavableData()
        {
            if (!objectID)
                objectID = GetComponent<ID>();

            SaveDataEntry entry = new SaveDataEntry()
            {
                Name = objectID.name,
                UniqueID = objectID.UniqueID.ToString(),
                LocalPosition = transform.localPosition,
                LocalScale = transform.localScale,
                LocalRotation = transform.localRotation.eulerAngles,
                Active = gameObject.activeSelf
               
            };

            IComponentSavable[] savables = GetComponents<IComponentSavable>();
            entry.SerializedComponents = new string[savables.Length];
            entry.SerializedComponentsTypes = new string[savables.Length];

            int index = 0;
            foreach (IComponentSavable savable in savables)
            {
                //Debug.Log($"Processing savable component: {savable.GetType().Name}");

                entry.SerializedComponentsTypes[index] = savable.GetType().Name;
                entry.SerializedComponents[index] = savable.GetSavableData();
                ++index;

            }

            return entry;
        }


    }



}
