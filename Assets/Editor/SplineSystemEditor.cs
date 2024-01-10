using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(SplineSystem))]
public class SplineSystemEditor : Editor
{
    private SerializedProperty sphereSize;
    private SerializedProperty curveResolution;
    private SerializedProperty loopCurve;
    private SerializedProperty waypointColor;
    private SerializedProperty segmentColor;
    private List<Transform> waypointsList = new List<Transform>();

    private enum ToolMode
    {
        Move,
        Rotate
    }

    private ToolMode currentToolMode = ToolMode.Move;
    private int selectedWaypointIndex = -1;

    private void OnEnable()
    {
        sphereSize = serializedObject.FindProperty("sphereSize");
        curveResolution = serializedObject.FindProperty("curveResolution");
        loopCurve = serializedObject.FindProperty("loopCurve");
        waypointColor = serializedObject.FindProperty("waypointColor");
        segmentColor = serializedObject.FindProperty("segmentColor");

        SplineSystem splineSystem = target as SplineSystem;
        waypointsList = new List<Transform>(splineSystem.GetChildWaypoints());
    
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(loopCurve);
        EditorGUILayout.PropertyField(sphereSize);
        EditorGUILayout.PropertyField(curveResolution);
        EditorGUILayout.PropertyField(waypointColor);
        EditorGUILayout.PropertyField(segmentColor);

        GUILayout.BeginHorizontal(); // Start a horizontal layout

        if (GUILayout.Button("Add Waypoint"))
        {
            AddWaypoint();
        }

        if (GUILayout.Button("Remove Selected"))
        {
            RemoveSelectedWaypoint();
        }

        GUILayout.EndHorizontal(); // End the horizontal layout

        if (GUILayout.Button("Set All Waypoints Y to 0"))
        {
            SetAllWaypointsYToZero();
        }

        //currentToolMode = (ToolMode)GUILayout.Toolbar((int)currentToolMode, new string[] { "Move Tool", "Rotate Tool" });

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        SplineSystem splineSystem = target as SplineSystem;
        Transform[] waypoints = splineSystem.GetChildWaypoints();

        Handles.color = splineSystem.waypointColor;

        for (int i = 0; i < waypoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            if (selectedWaypointIndex == i)
            {
                Vector3 newPosition = Handles.PositionHandle(waypoints[i].position, Quaternion.identity);
                Quaternion newRotation = Handles.RotationHandle(waypoints[i].rotation, waypoints[i].position);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypoints[i], "Move Waypoint");
                    waypoints[i].position = newPosition;
                    Undo.RecordObject(waypoints[i], "Rotate Waypoint");
                    waypoints[i].rotation = newRotation;
                }

                //if (currentToolMode == ToolMode.Move)
                //{
                //    Vector3 newPosition = Handles.PositionHandle(waypoints[i].position, Quaternion.identity);
                //    Quaternion newRotation = Handles.RotationHandle(waypoints[i].rotation, waypoints[i].position);
                //    if (EditorGUI.EndChangeCheck())
                //    {
                //        Undo.RecordObject(waypoints[i], "Move Waypoint");
                //        waypoints[i].position = newPosition;
                //        Undo.RecordObject(waypoints[i], "Rotate Waypoint");
                //        waypoints[i].rotation = newRotation;
                //    }
                //}
                //else if (currentToolMode == ToolMode.Rotate)
                //{
                //    Quaternion newRotation = Handles.RotationHandle(waypoints[i].rotation, waypoints[i].position);
                //    if (EditorGUI.EndChangeCheck())
                //    {
                //        Undo.RecordObject(waypoints[i], "Rotate Waypoint");
                //        waypoints[i].rotation = newRotation;
                //    }
                //}
            }

            if (Handles.Button(waypoints[i].position, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap))
            {
                selectedWaypointIndex = i;
            }
        }
    }

    private void RemoveSelectedWaypoint()
    {
        SplineSystem splineSystem = target as SplineSystem;
        List<Transform> waypoints = new List<Transform>(splineSystem.GetChildWaypoints());

        if (selectedWaypointIndex >= 0 && selectedWaypointIndex < waypoints.Count)
        {
            Transform selectedWaypoint = waypoints[selectedWaypointIndex];

            Undo.DestroyObjectImmediate(selectedWaypoint.gameObject);
            waypoints.RemoveAt(selectedWaypointIndex);

            // Renumber all remaining waypoints in the list
            for (int i = 0; i < waypoints.Count; i++)
            {
                waypoints[i].name = "Waypoint " + (i + 1);
            }

            // Update the hierarchy order to match the sorted list
            for (int i = 0; i < waypoints.Count; i++)
            {
                waypoints[i].SetSiblingIndex(i);
            }

            waypointsList = waypoints;
            selectedWaypointIndex = -1;

            SceneView.RepaintAll();
        }
    }




    private void AddWaypoint()
    {
        SplineSystem splineSystem = target as SplineSystem;
        List<Transform> waypoints = new List<Transform>(splineSystem.GetChildWaypoints());

        Transform newWaypoint = new GameObject("Waypoint " + (waypoints.Count + 1)).transform; // Add numbering
        newWaypoint.parent = splineSystem.transform;

        if (waypoints.Count > 0 && selectedWaypointIndex >= 0 && selectedWaypointIndex < waypoints.Count)
        {
            Transform selectedWaypoint = waypoints[selectedWaypointIndex];

            // Insert the new waypoint at the selected waypoint's position in the list
            newWaypoint.position = selectedWaypoint.position;

            waypoints.Insert(selectedWaypointIndex + 1, newWaypoint); // Insert into the list at the specified index
        }
        else if (waypoints.Count > 0)
        {
            newWaypoint.position = waypoints[waypoints.Count - 1].position; // Set new waypoint position as the last waypoint's position
        }
        else
        {
            newWaypoint.position = Vector3.zero; // If no waypoints exist, set to the origin
        }

        // Renumber all waypoints in the list
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].name = "Waypoint " + (i + 1);
        }

        // Sort the waypoints list based on their names
        waypoints.Sort((a, b) => string.Compare(a.name, b.name));

        // Update the hierarchy order to match the sorted list
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].SetSiblingIndex(i);
        }

        // Update waypointsList and selectedWaypointIndex
        waypointsList = waypoints;
        selectedWaypointIndex = waypoints.IndexOf(newWaypoint);

        Undo.RegisterCreatedObjectUndo(newWaypoint.gameObject, "Add Waypoint");

        SceneView.RepaintAll();
    }




    private void SetAllWaypointsYToZero()
    {
        SplineSystem splineSystem = target as SplineSystem;
        Transform[] waypoints = splineSystem.GetChildWaypoints();

        Undo.RecordObjects(waypoints, "Set All Waypoints Y to 0");

        foreach (var waypoint in waypoints)
        {
            Vector3 newPosition = waypoint.position;
            newPosition.y = 0f;
            waypoint.position = newPosition;
        }
    }
}
