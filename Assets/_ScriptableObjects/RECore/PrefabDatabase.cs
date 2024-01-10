using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [CreateAssetMenu(fileName = "PrefabDatabase", menuName = "ResidentEvilClone/PrefabDatabase", order = 1)]
    public class PrefabDatabase : ScriptableObject
    {
        [System.Serializable]
        public class PrefabCategory
        {
            public string categoryName; // For example: "UI", "Player", "Camera", etc.
            public GameObject[] prefabs;
        }

        public PrefabCategory[] prefabCategories;
    }

}
