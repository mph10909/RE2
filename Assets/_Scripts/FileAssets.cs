using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class FileAssets : MonoBehaviour
    {
        public static FileAssets Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [TextArea(3,10)]public string[] churchDiary;
        [TextArea(3,10)]public string[] restaurantDiary;
        [TextArea(3,10)]public string[] patientChart;
        [TextArea(3,10)]public string[] umbrellaFile;
        [TextArea(3,10)]public string[] janitorDiary;
        [TextArea(3,10)]public string[] lottsDiary;
        [TextArea(3,10)]public string[] racoonCityReport;
        [TextArea(3,10)]public string[] churchArcadeEntry;
        [TextArea(3,10)]public string[] serversMemo;
        [TextArea(3,10)]public string[] theaterNote;

    }
}
