using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ID))]
public class ItemIDGenerator : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    ID id = (ID)target;
    //    if (id.UniqueID == 0)
    //    {
    //        id.UniqueID = System.Guid.NewGuid().GetHashCode();
    //    }
    //}
}
