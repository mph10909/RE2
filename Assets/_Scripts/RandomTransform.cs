using UnityEngine;
using UnityEditor;

public class RandomTransform : MonoBehaviour
{
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float minRotation = 0f;
    public float maxRotation = 360f;

    void Awake()
    {
        GenerateRandomTransform();
    }

    public void GenerateRandomTransform()
    {
        // Generate random scale and rotation
        float randomScale = Random.Range(minScale, maxScale);
        float randomRotation = Random.Range(minRotation, maxRotation);

        // Apply the random scale and rotation to the object
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        transform.localRotation = Quaternion.Euler(transform.eulerAngles.x  , randomRotation, transform.eulerAngles.y);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // Clamp the limits in the editor to avoid errors
        minScale = Mathf.Max(0f, minScale);
        maxScale = Mathf.Max(minScale, maxScale);
        maxRotation = Mathf.Clamp(maxRotation, minRotation, 360f);
    }

    [CustomEditor(typeof(RandomTransform))]
    public class RandomTransformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RandomTransform randomTransform = (RandomTransform)target;
            if (GUILayout.Button("Generate"))
            {
                randomTransform.GenerateRandomTransform();
            }
        }
    }
#endif
}

