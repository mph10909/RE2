using UnityEngine;
using UnityEditor;
using System.Collections.Generic;



public class LadderBuilder : EditorWindow
{


    private List<LadderSystem> ladderSystems = new List<LadderSystem>();
    private int selectedLadderSystemIndex = -1;
    private LadderSystemData ladderData;


    [MenuItem("Tools/Create Ladder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LadderBuilder));
    }

    private void OnEnable()
    {
        // Load the scriptable object or create a new one if it doesn't exist
        string assetPath = "Assets/LadderSystemData.asset";
        ladderData = AssetDatabase.LoadAssetAtPath<LadderSystemData>(assetPath);

        if (ladderData == null)
        {
            ladderData = CreateInstance<LadderSystemData>();
            AssetDatabase.CreateAsset(ladderData, assetPath);
            AssetDatabase.SaveAssets();
        }
        else
        {
            // If there's data, update the in-memory list.
            ladderSystems = ladderData.ladderSystems;
        }
        SceneView.duringSceneGui += this.OnSceneView;
    }


    private void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneView;
    }

    private void OnSceneView(SceneView sceneView)
    {
        // This is where you put the logic from your OnSceneGUI method
        DrawGizmosOnLadderEnds();
    }


    private void OnGUI()
    {
        GUILayout.Label("Ladder Builder", EditorStyles.boldLabel);

        if (ladderSystems.Count == 0)
        {
            EditorGUILayout.HelpBox("No ladder systems created. Click 'Create Ladder System' to create a new ladder system.", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < ladderSystems.Count; i++)
            {
                DrawLadderSystemGUI(i, ladderSystems[i]);
            }
        }

        if (GUILayout.Button("Create Ladder System"))
        {
            LadderSystem newLadderSystem = new LadderSystem();
            newLadderSystem.ladderParent = new GameObject("Ladder System " + ladderSystems.Count);
            ladderSystems.Add(newLadderSystem);
            ladderData.ladderSystems = ladderSystems;  // Synchronize with asset data
            EditorUtility.SetDirty(ladderData);        // Mark the asset data as modified
            AssetDatabase.SaveAssets();                // Save changes to the asset data
        }

    }

    private void DrawLadderSystemGUI(int index, LadderSystem ladderSystem)
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        ladderSystem.ladderPiecePrefab = EditorGUILayout.ObjectField("Ladder Piece Prefab", ladderSystem.ladderPiecePrefab, typeof(GameObject), false) as GameObject;

        // Display a preview of the ladder piece prefab
        if (ladderSystem.ladderPiecePrefab != null)
        {
            Texture2D previewTexture = AssetPreview.GetAssetPreview(ladderSystem.ladderPiecePrefab);
            if (previewTexture != null)
            {
                GUILayout.Box(previewTexture, GUILayout.Width(64), GUILayout.Height(64));
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        // "Add Ladder Piece" button
        if (GUILayout.Button("Add Ladder Piece"))
        {
            if (ladderSystem.ladderPiecePrefab != null)
            {
                GameObject ladderPiece = Instantiate(ladderSystem.ladderPiecePrefab);
                ladderPiece.name = "Ladder Piece " + ladderSystem.ladderPieces.Count;
                ladderPiece.transform.parent = ladderSystem.ladderParent.transform;
                ladderSystem.ladderPieces.Add(ladderPiece);
                UpdateLadderPiecePositions(ladderSystem);
            }
        }

        // Display the number of ladder pieces
        GUILayout.FlexibleSpace();
        GUILayout.Label(ladderSystem.ladderPieces.Count.ToString());
        GUILayout.FlexibleSpace();

        // "Remove Ladder Piece" button
        if (GUILayout.Button("Remove Ladder Piece"))
        {
            if (ladderSystem.ladderPieces.Count > 0)
            {
                GameObject lastLadderPiece = ladderSystem.ladderPieces[ladderSystem.ladderPieces.Count - 1];
                ladderSystem.ladderPieces.RemoveAt(ladderSystem.ladderPieces.Count - 1);
                DestroyImmediate(lastLadderPiece);
                UpdateLadderPiecePositions(ladderSystem);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ladder Piece Distance", GUILayout.Width(150));
        float newLadderPieceDistance = EditorGUILayout.Slider(ladderSystem.ladderPieceDistance, 0.0f, 10.0f);

        if (Mathf.Abs(newLadderPieceDistance - ladderSystem.ladderPieceDistance) > 0.001f)
        {
            ladderSystem.ladderPieceDistance = newLadderPieceDistance;
            ladderSystem.isDraggingSlider = true;
        }
        EditorGUILayout.EndHorizontal();

        if (ladderSystem.isDraggingSlider)
        {
            UpdateLadderPiecePositions(ladderSystem);
            ladderSystem.isDraggingSlider = false;
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Select Ladder System"))
            {
                selectedLadderSystemIndex = index;
                Selection.activeGameObject = ladderSystem.ladderParent; // This selects the GameObject in the Unity editor.
            }

            if (GUILayout.Button("Delete Ladder System"))
            {
                selectedLadderSystemIndex = index;
                DeleteSelectedLadderSystem();
            }
        }
        EditorGUILayout.EndHorizontal();

    }

    

    private void DeleteSelectedLadderSystem()
    {
        if (selectedLadderSystemIndex >= 0 && selectedLadderSystemIndex < ladderSystems.Count)
        {
            DestroyImmediate(ladderSystems[selectedLadderSystemIndex].ladderParent);
            ladderSystems.RemoveAt(selectedLadderSystemIndex);
            ladderData.ladderSystems = ladderSystems;  // Synchronize with asset data
            EditorUtility.SetDirty(ladderData);        // Mark the asset data as modified
            AssetDatabase.SaveAssets();                // Save changes to the asset data
            selectedLadderSystemIndex = -1;
        }
    }

    private float GetPrefabHeight(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0f;

        float minY = float.PositiveInfinity;
        float maxY = float.NegativeInfinity;

        foreach (Renderer renderer in renderers)
        {
            minY = Mathf.Min(minY, renderer.bounds.min.y);
            maxY = Mathf.Max(maxY, renderer.bounds.max.y);
        }

        return maxY - minY;
    }

    private void UpdateLadderPiecePositions(LadderSystem ladderSystem)
    {
        float prefabHeight = GetPrefabHeight(ladderSystem.ladderPiecePrefab);

        for (int i = 0; i < ladderSystem.ladderPieces.Count; i++)
        {
            Vector3 localPosition = Vector3.up * (prefabHeight + ladderSystem.ladderPieceDistance) * i;
            ladderSystem.ladderPieces[i].transform.localPosition = localPosition;
        }
    }

    private void DrawGizmosOnLadderEnds()
    {
        if (ladderSystems == null || ladderSystems.Count == 0)
            return;

        foreach (LadderSystem ladderSystem in ladderSystems)
        {
            if (ladderSystem.ladderPieces.Count > 0)
            {
                Handles.color = Color.magenta;

                GameObject firstPiece = ladderSystem.ladderPieces[0];
                GameObject lastPiece = ladderSystem.ladderPieces[ladderSystem.ladderPieces.Count - 1];

                if (firstPiece != null)
                {
                    AdjustBounds(firstPiece, ladderSystem); // Assuming AdjustBounds is your current method for drawing and adjusting the bounding box
                    AdjustOrAddBoxCollider(firstPiece, ladderSystem);
                }

                if (lastPiece != null && ladderSystem.ladderPieces.Count > 1)
                {
                    AdjustBounds(lastPiece, ladderSystem);
                    AdjustOrAddBoxCollider(lastPiece, ladderSystem);
                }

                // Drawing arrows (retaining the previous arrow-drawing code for clarity)
                if (ladderSystem.ladderPieces.Count > 1)
                {
                    DrawArrowBetweenPieces(firstPiece, ladderSystem.ladderPieces[1], true); // Up arrow
                    DrawArrowBetweenPieces(lastPiece, ladderSystem.ladderPieces[ladderSystem.ladderPieces.Count - 2], false); // Down arrow
                }

                CleanupMiddlePieceColliders(ladderSystem);
            }
        }
    }


    private void AdjustBounds(GameObject piece, LadderSystem ladderSystem)
    {
        Vector3 centerPosition = piece.transform.position + ladderSystem.boundsCenter;

        // Draw bounding box
        Handles.DrawWireCube(centerPosition, ladderSystem.boundsSize);

        // Slider handles for size adjustment
        Vector3 newHandlePosition;

        // X axis
        newHandlePosition = Handles.Slider(centerPosition + new Vector3(ladderSystem.boundsSize.x / 2, 0, 0), Vector3.right);
        ladderSystem.boundsSize.x = (newHandlePosition - centerPosition).x * 2;

        // Y axis
        newHandlePosition = Handles.Slider(centerPosition + new Vector3(0, ladderSystem.boundsSize.y / 2, 0), Vector3.up);
        ladderSystem.boundsSize.y = (newHandlePosition - centerPosition).y * 2;

        // Z axis
        newHandlePosition = Handles.Slider(centerPosition + new Vector3(0, 0, ladderSystem.boundsSize.z / 2), Vector3.forward);
        ladderSystem.boundsSize.z = (newHandlePosition - centerPosition).z * 2;

    }



    private void DrawArrowBetweenPieces(GameObject startPiece, GameObject endPiece, bool pointingUp)
    {
        Vector3 startPosition = startPiece.transform.position;
        Vector3 endPosition = endPiece.transform.position;
        Vector3 direction = pointingUp ? Vector3.up : Vector3.down;

        float arrowSize = 1f;
        float arrowPosition = pointingUp ? 0.75f : 0.25f;

        Vector3 arrowStartPoint = Vector3.Lerp(startPosition, endPosition, arrowPosition);

        Handles.ArrowHandleCap(0, arrowStartPoint, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
    }

    private void AdjustOrAddBoxCollider(GameObject ladderPiece, LadderSystem ladderSystem)
    {
        BoxCollider boxCollider = ladderPiece.GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            boxCollider = ladderPiece.AddComponent<BoxCollider>();
        }

        // Adjust for the object's scale
        Vector3 invertedScale = new Vector3(1f / ladderPiece.transform.localScale.x,
                                            1f / ladderPiece.transform.localScale.y,
                                            1f / ladderPiece.transform.localScale.z);

        Vector3 adjustedBoundsSize = Vector3.Scale(ladderSystem.boundsSize, invertedScale);
        Vector3 adjustedBoundsCenter = Vector3.Scale(ladderSystem.boundsCenter, invertedScale);

        // Convert from global space to local space
        Vector3 localSize = ladderPiece.transform.InverseTransformDirection(adjustedBoundsSize);
        Vector3 localCenter = ladderPiece.transform.InverseTransformPoint(ladderPiece.transform.position + adjustedBoundsCenter);

        boxCollider.center = localCenter;
        boxCollider.size = localSize;
    }



    private void CleanupMiddlePieceColliders(LadderSystem ladderSystem)
    {
        for (int i = 1; i < ladderSystem.ladderPieces.Count - 1; i++)
        {
            BoxCollider collider = ladderSystem.ladderPieces[i].GetComponent<BoxCollider>();
            if (collider)
            {
                DestroyImmediate(collider);
            }
        }
    }





}
