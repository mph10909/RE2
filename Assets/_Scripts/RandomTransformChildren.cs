using UnityEngine;
using UnityEditor;

public class RandomTransformChildren : MonoBehaviour
{

    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float minRotation = 0f;
    public float maxRotation = 360f;
    [SerializeField] bool canScale, rotateX, rotateY, rotateZ, fullRotate;


    //void Awake()
    //{
    //    GenerateRandomTransform();
    //}

    public void GenerateRandomTransform()
    {
        foreach(Transform child in transform)
        {
        // Generate random scale and rotation
        float randomScale = Random.Range(minScale, maxScale);
        float randomRotation = Random.Range(minRotation, maxRotation);

        // Apply the random scale and rotation to the object
        if(canScale)child.localScale = new Vector3(randomScale, randomScale, randomScale);
        if (rotateX) child.eulerAngles = new Vector3(randomScale, child.eulerAngles.y, child.eulerAngles.z);
        if (rotateY) child.eulerAngles = new Vector3(child.eulerAngles.x, randomScale, child.eulerAngles.z);
        if (rotateZ) child.eulerAngles = new Vector3(child.eulerAngles.x, child.eulerAngles.y, randomScale );
        if (fullRotate) child.localRotation = Quaternion.Euler(child.eulerAngles.x, randomRotation, child.eulerAngles.y);
        }

    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // Clamp the limits in the editor to avoid errors
        minScale = Mathf.Max(0f, minScale);
        maxScale = Mathf.Max(minScale, maxScale);
        maxRotation = Mathf.Clamp(maxRotation, minRotation, 360f);
    }

    [CustomEditor(typeof(RandomTransformChildren))]
    public class RandomTransformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RandomTransformChildren randomTransform = (RandomTransformChildren)target;
            if (GUILayout.Button("Generate"))
            {
                randomTransform.GenerateRandomTransform();
            }
        }
    }
#endif
}

