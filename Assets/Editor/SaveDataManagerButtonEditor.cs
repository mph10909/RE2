#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace ResidentEvilClone
{
    [CustomEditor(typeof(SaveDataManagerButton))]
    public class SaveDataManagerButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SaveDataManagerButton script = (SaveDataManagerButton)target;

            if (GUILayout.Button("Save Data"))
            {
                script.SaveData();
            }

            if (GUILayout.Button("Load Data"))
            {
                script.LoadData();
            }
        }
    }
}

#endif
