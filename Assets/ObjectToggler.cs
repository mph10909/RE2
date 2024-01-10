using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ObjectToggler : MonoBehaviour
    {
        public GameObject objectToToggle; // Drag the object you want to toggle here in the editor

        // This function toggles the object's active state
        public void ToggleObject()
        {
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            }
        }
    }
}
