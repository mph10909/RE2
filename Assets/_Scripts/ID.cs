using UnityEngine;
using System;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ID : MonoBehaviour
{
    public string UniqueID;
    [Tooltip("This indicates the Id doesn't need to be different from the prefab since there won't be more instances")]
    public bool IsUniqueInstance;

    public string Id => UniqueID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(Id))
            RegenerateId();
    }

    [ContextMenu("Regenerate Id")]
    public void RegenerateId()
    {
        Guid id = Guid.NewGuid();
        UniqueID = id.ToString();
    }

    public void SetId(string id)
    {
        UniqueID = id;
    }
}
