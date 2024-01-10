using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
using ResidentEvilClone;

public class PickupObjectCreator : EditorWindow
{
    string itemName = "New Item";
    GameObject prefabToAdd;
    GameObject observerPrefab;
    Transform inspectionLocation;
    GameObject model;
    Texture2D modelPreviewTexture;
    Editor modelEditor; // For the interactive preview
    Vector3 clickableSize = new Vector3 (10,10,10);
    Vector3 detectionSize = new Vector3(15,15,15);
    float clickableRadius;
    float detectionRadius;

    private GUIStyle headerStyle;
    private GUIStyle boxStyle;

    [MenuItem("Tools/Pickup Object Creator")]
    public static void ShowWindow()
    {
        GetWindow<PickupObjectCreator>("Pickup Object Creator");
    }

    enum ColliderType
    {
        Sphere,
        Box,
        Capsule
    }

    ColliderType selectedColliderType = ColliderType.Sphere; // Default to Sphere

    void OnGUI()
    {
        if (headerStyle == null) // Check if the style is already created to avoid recreation every OnGUI call
        {
            headerStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
        }

        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 10, 10), // Add some padding to make things look nicer
            };
        }

        GUILayout.Space(10); // Add some space at the very top for padding.

        // Header
        EditorGUILayout.LabelField("Create a New Pickup Object", headerStyle);

        GUILayout.Space(20); // Add space before the first control set.

        // Item Name Field
        GUILayout.BeginVertical(boxStyle); // Begin a vertical group with a box style for a border.
        itemName = EditorGUILayout.TextField("Item Name", itemName);
        GUILayout.EndVertical();

        GUILayout.Space(10); // Add space between sections.

        // Model Field
        GUILayout.BeginVertical(boxStyle); // Model field with a box.
        EditorGUI.BeginChangeCheck();
        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck() && model != null)
        {
            // Destroy the previous editor if there was one
            if (modelEditor != null) DestroyImmediate(modelEditor);
            modelEditor = Editor.CreateEditor(model);
        }
        GUILayout.EndVertical();

        GUILayout.Space(10); // Add space before the preview area.

        // Model Preview
        if (model != null && modelEditor != null)
        {
            GUILayout.BeginVertical(boxStyle); // Start the preview section.
            EditorGUILayout.LabelField("Model Preview", EditorStyles.boldLabel);
            Rect r = GUILayoutUtility.GetRect(256, 256, GUILayout.ExpandWidth(true)); // Make the preview expand to the width of the window.
            modelEditor.OnInteractivePreviewGUI(r, GUIStyle.none);
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical(boxStyle);
            EditorGUILayout.LabelField("Model Preview", EditorStyles.boldLabel);
            GUILayout.Label("No model selected for preview.", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();
        }
        GUILayout.Space(10); // Add space before the collider selection.

        // Collider Type Selection
        GUILayout.BeginVertical(boxStyle);
        selectedColliderType = (ColliderType)EditorGUILayout.EnumPopup("Collider Type", selectedColliderType);
        GUILayout.EndVertical();

        GUILayout.Space(10); // Add space between sections.

        // Buttons
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Center the button by pushing it from the left
        if (GUILayout.Button("Create Pickup Object", GUILayout.Width(200))) // You can set a width for the button
        {
            CreatePickupObject();
        }
        GUILayout.FlexibleSpace(); // And pushing it from the right
        GUILayout.EndHorizontal();

        GUILayout.Space(10); // Bottom padding space.
    }

    private void CreatePickupObject()
    {
        // Check if the user has provided a specific name, if not, use the model's name.
        if (string.IsNullOrEmpty(itemName) && model != null)
        {
            itemName = model.name; // Default to the model's name if no specific name was provided.
        }
        else if (string.IsNullOrEmpty(itemName))
        {
            EditorUtility.DisplayDialog("Item Name Not Set", "Please enter a name for the item or assign a model to derive the name from.", "OK");
            return; // Stop execution if no name is set and no model is provided.
        }

        // Use the model's name for the item if a model is assigned
        itemName = model.name;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GameObject pickupObject = new GameObject(itemName);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ObjectPickup interactableComponent = pickupObject.AddComponent<ObjectPickup>();
        pickupObject.AddComponent<ID>();
        pickupObject.AddComponent<ResidentEvilClone.SavableObject>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GameObject inspectionLocationObj = new GameObject("InspectionLocation");
        inspectionLocationObj.transform.SetParent(pickupObject.transform, false);
        inspectionLocation = inspectionLocationObj.transform;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GameObject clickable = new GameObject("Clickable");
        clickable.transform.SetParent(pickupObject.transform, false);
        AddCollider(clickable, clickableSize);

        // Add the PlayerDetection script and configure it
        PlayerDetect playerDetection = clickable.AddComponent<PlayerDetect>();
        if (playerDetection.clickableData == null)
            playerDetection.clickableData = new ClickableData(); // Make sure it's instantiated before setting its properties

        playerDetection.clickableData.inspectionLocation = inspectionLocation;

        // Now add a persistent listener safely
        if (playerDetection != null && interactableComponent != null)
        {
            playerDetection.Interact = new UnityEvent<IInteractor>();
            //UnityEventTools.AddPersistentListener(playerDetection.Interact, interactableComponent.Interact);
        }
        else
        {
            Debug.LogError("One or more required components are null.");
        }

        clickable.SetActive(false);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        GameObject observer = new GameObject("Observer");
        observer.transform.SetParent(pickupObject.transform, false);
        AddCollider(observer, detectionSize);

        observer.layer = LayerMask.NameToLayer("Ignore Raycast");

        TriggerObserver triggerObserver = observer.AddComponent<TriggerObserver>();
        triggerObserver.TriggerEnter = new UnityEvent();
        triggerObserver.TriggerExit = new UnityEvent();

        UnityEventTools.AddPersistentListener(triggerObserver.TriggerEnter, playerDetection.EnableClickable);
        UnityEventTools.AddPersistentListener(triggerObserver.TriggerExit, playerDetection.DisableClickable);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (model != null)
        {
            // Create a new child transform within the pickup object with the name of the model.
            GameObject modelContainer = new GameObject(model.name);
            modelContainer.transform.SetParent(pickupObject.transform, false); // Set the newly created GameObject as a child of pickupObject.

            // Instantiate the model as a child of modelContainer.
            GameObject modelInstance = Instantiate(model, modelContainer.transform.position, Quaternion.identity, modelContainer.transform);
            modelInstance.name = model.name; // Optionally, ensure the instantiated model has the correct name.
        }
        else
        {
            Debug.LogWarning("No model assigned to the Pickup Object Creator.");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.FrameSelected();
        }

        // Select the new object in the editor.
        Selection.activeGameObject = pickupObject;
    }

    private void AddCollider(GameObject target, Vector3 boxSize )
    {
        switch (selectedColliderType)
        {
            case ColliderType.Sphere:
                SphereCollider sphereCollider = target.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = 5;
                break;
            case ColliderType.Box:
                BoxCollider boxCollider = target.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                boxCollider.size = boxSize; // Replace with your desired size
                break;
            case ColliderType.Capsule:
                CapsuleCollider capsuleCollider = target.AddComponent<CapsuleCollider>();
                capsuleCollider.isTrigger = true;
                capsuleCollider.radius = 5;
                capsuleCollider.height = 10; // Replace with your desired height
                break;
        }
    }

}
