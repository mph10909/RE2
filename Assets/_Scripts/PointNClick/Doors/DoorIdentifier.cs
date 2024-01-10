using System;
using UnityEngine;

namespace ResidentEvilClone
{
    [CreateAssetMenu(fileName = "DoorIdentifier", menuName = "Scene/DoorIdentifier", order = 1)]
    public class DoorIdentifier : ScriptableObject
    {
        
        public string identifier;

        [ContextMenu("GenID")]

        public void GenID()
        {
        identifier = Guid.NewGuid().ToString();
        }


    }
}

