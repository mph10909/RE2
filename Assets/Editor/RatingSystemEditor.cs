using UnityEngine;
using UnityEditor;
using ResidentEvilClone;

[CustomEditor(typeof(SetResults))]
public class RatingSystemEditor : Editor
{
        private bool showManagers = true;
        private bool showText = true;
        private bool showRatings = true;


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical(GUI.skin.box);

        showManagers = EditorGUI.BeginFoldoutHeaderGroup(EditorGUILayout.GetControlRect(), showManagers, "Managers");

        if (showManagers)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            DrawField("Time Manager", serializedObject.FindProperty("timeManager"));
            DrawField("Save Manager", serializedObject.FindProperty("saveManager"));
            EditorGUILayout.EndVertical();
        }
        EditorGUI.EndFoldoutHeaderGroup();

        GUILayout.Space(10f);

        showText = EditorGUI.BeginFoldoutHeaderGroup(EditorGUILayout.GetControlRect(), showText, "Text Fields");

        if (showText)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            DrawField("Total Time Text", serializedObject.FindProperty("totalTimeText"));
            DrawField("Results Text", serializedObject.FindProperty("results_Text"));
            DrawField("Number of Saves Text", serializedObject.FindProperty("numberOfSavesText"));
            EditorGUILayout.EndVertical();
        }
        EditorGUI.EndFoldoutHeaderGroup();

        GUILayout.Space(10f);

        showRatings = EditorGUI.BeginFoldoutHeaderGroup(EditorGUILayout.GetControlRect(), showRatings, "Ratings Fields");

        if (showRatings)
        {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        DrawRow("S", "Time", serializedObject.FindProperty("sTime"), "Save", serializedObject.FindProperty("sSave"));
        DrawRow("A", "Time", serializedObject.FindProperty("aTime"), "Save", serializedObject.FindProperty("aSave"));
        DrawRow("B", "Time", serializedObject.FindProperty("bTime"), "Save", serializedObject.FindProperty("bSave"));
        GUILayout.EndVertical();
        }
        EditorGUI.EndFoldoutHeaderGroup();

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRow(string rating, string criteria, SerializedProperty time, string savecriteria, SerializedProperty save)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(rating + " Rating :", GUILayout.Width(50));
        GUILayout.Label(criteria, GUILayout.Width(40));
        EditorGUILayout.PropertyField(time, GUIContent.none, GUILayout.Width(60));
        GUILayout.Label(savecriteria, GUILayout.Width(40));
        EditorGUILayout.PropertyField(save, GUIContent.none, GUILayout.Width(60));
        GUILayout.EndHorizontal();
    }

    private void DrawField(string label, SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property, new GUIContent(label));
    }
}
