using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PersistAcrossScenes : MonoBehaviour
    {
        private static bool isInstantiated = false;
        public static bool IsParentDestroyed;

        private void Awake()
        {
            if (!isInstantiated)
            {
                DontDestroyOnLoad(this.gameObject);
                isInstantiated = true;
                IsParentDestroyed = false;
            }
            else
            {
                IsParentDestroyed = true;
                Destroy(this.gameObject);
            }
        }
    }
}
