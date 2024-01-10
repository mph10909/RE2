using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FootStepInfo
{
    [HideInInspector] public string name;
    public FootStepSurface footStepSurface;
    public FootSteps footSteps;
}

[CreateAssetMenu(fileName = "FootStepManager", menuName = "ResidentEvilClone/FootSteps/FootStepManager")]
public class FootStepData : ScriptableObject
{
    public List<FootStepInfo> footStepData = new List<FootStepInfo>();

#if UNITY_EDITOR
    void Reset() { OnValidate(); }

    void OnValidate()
    {
        for (int i = 0; i < footStepData.Count; i++)
        {
            if (footStepData[i].footStepSurface != null)
            {
                footStepData[i].name = footStepData[i].footStepSurface.name;
            }
        }
    }
#endif
}

