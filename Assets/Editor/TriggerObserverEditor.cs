using UnityEngine;
using UnityEditor;
using UnityEditor.Events;

[CustomEditor(typeof(TriggerObserver))]
public class TriggerObserverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TriggerObserver script = (TriggerObserver)target;

        if (GUILayout.Button("Autofill Events"))
        {
            AutofillEvents(script);
        }
    }

    private void AutofillEvents(TriggerObserver script)
    {
        // Find the PlayerDetect script in the inactive children
        PlayerDetect pd = FindPlayerDetectInChildren(script.transform.parent);

        if (pd == null)
        {
            Debug.LogWarning("PlayerDetect script not found in the inactive children!");
            return;
        }

        // Clear current listeners to avoid duplicates
        script.TriggerEnter.RemoveAllListeners();
        script.TriggerExit.RemoveAllListeners();

        // Add new listeners for TriggerEnter
        UnityEventTools.AddPersistentListener(script.TriggerEnter, pd.EnableClickable);

        // Add new listeners for TriggerExit
        UnityEventTools.AddPersistentListener(script.TriggerExit, pd.DisableClickable);
    }

    private PlayerDetect FindPlayerDetectInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            PlayerDetect pd = child.GetComponent<PlayerDetect>();
            if (pd != null)
            {
                return pd;
            }

            // Search in the grandchildren (regardless of their active state)
            PlayerDetect pdInChildren = FindPlayerDetectInChildren(child);
            if (pdInChildren != null)
            {
                return pdInChildren;
            }
        }

        return null;
    }
}
