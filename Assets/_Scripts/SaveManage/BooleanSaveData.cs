using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ResidentEvilClone
{
    public class BooleanSaveData : MonoBehaviour
    {
        private SaveManager _saveManager;
        public SaveManager SaveManger => _saveManager;

        public List<string> trueBooleans;
        public List<string> falseBooleans;
        //public List<string> ignoreScripts;
        public List<MonoBehaviour> ignoreScripts;

        [SerializeField]

        public void OnCreated(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }


        public void LoadData(SaveData saveData)
        {
            if (saveData.booleanData == null)
            {
                saveData.booleanData = new List<SaveData.BooleanData>();
            }

            foreach (SaveData.BooleanData boolData in saveData.booleanData)
            {
                if (boolData.objName == this.gameObject.name)
                {
                    GameObject gameObject = this.gameObject;

                    // Set the true booleans
                    foreach (string boolName in boolData.trueBools)
                    {
                        string[] split = boolName.Split('.');
                        string scriptName = split[0];
                        string fieldName = split[1];

                        MonoBehaviour script = gameObject.GetComponent(scriptName) as MonoBehaviour;
                        if (script != null)
                        {
                            FieldInfo field = script.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                            if (field != null && field.FieldType == typeof(bool))
                            {
                                field.SetValue(script, true);
                            }
                        }
                    }

                    // Set the false booleans
                    foreach (string boolName in boolData.falseBools)
                    {
                        string[] split = boolName.Split('.');
                        string scriptName = split[0];
                        string fieldName = split[1];

                        MonoBehaviour script = gameObject.GetComponent(scriptName) as MonoBehaviour;
                        if (script != null)
                        {
                            FieldInfo field = script.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                            if (field != null && field.FieldType == typeof(bool))
                            {
                                field.SetValue(script, false);
                            }
                        }
                    }
                }
            }
        }

        public void SaveData(SaveData saveData)
        {
            SaveData.BooleanData boolData = new SaveData.BooleanData();

            boolData.objName = this.gameObject.name;
            boolData.obj = this.gameObject;
            SaveBooleans();

            boolData.trueBools = trueBooleans;
            boolData.falseBools = falseBooleans;

            saveData.booleanData.Add(boolData);
        }

        private void SaveBooleans()
        {
            trueBooleans = new List<string>();
            falseBooleans = new List<string>();

            trueBooleans.Clear();
            falseBooleans.Clear();

            MonoBehaviour[] scripts = this.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (!ignoreScripts.Contains(script))
                {
                    FieldInfo[] fields = script.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (FieldInfo field in fields)
                    {
                        if (field.FieldType == typeof(bool))
                        {
                            if ((bool)field.GetValue(script))
                            {
                                trueBooleans.Add(script.GetType().Name + "." + field.Name);
                            }
                            else
                            {
                                falseBooleans.Add(script.GetType().Name + "." + field.Name);
                            }
                        }
                    }
                }
            }
        }
    }

}
