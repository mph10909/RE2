    using UnityEngine;
    using UnityEditor;

    [ExecuteInEditMode]
    public class InspectionLocationAdder : MonoBehaviour
    {
    void OnValidate()
    {
        // Check if the script is attached to a prefab instance
        if (Application.isPlaying || PrefabUtility.IsPartOfPrefabInstance(this.gameObject))
        {
            return;
        }
        // check if the child GameObject already exists
        Transform inspectionLocationTransform = this.transform.Find("InspectionLocation");
        if (inspectionLocationTransform == null)
        {
            GameObject inspectionLocation = new GameObject("InspectionLocation");
            inspectionLocation.transform.SetParent(this.transform);
            inspectionLocation.transform.localPosition = Vector3.zero;
        }
    }

}
    
