using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class Loader : MonoBehaviour
    {
        public static int saveAmount;
        public static int firstAidSprayAmount;
        public static string SaveFile;
        public static bool Loaded;

        public static int SaveAmount { get { return saveAmount; } set { saveAmount = value; } }

    }
}
