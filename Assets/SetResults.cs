using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{

    public class SetResults : MonoBehaviour
    {
        string results;
        float totalTime;  
        int numberOfSaves;

        [SerializeField] TimeManager timeManager;
        [SerializeField] SaveManager saveManager;

        [SerializeField] Text totalTimeText;
        [SerializeField] Text results_Text;
        [SerializeField] Text numberOfSavesText;

        public float sTime;
        public int sSave;
        public float aTime;
        public int aSave;
        public float bTime;
        public int bSave;

        void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            numberOfSaves = Loader.SaveAmount;
            totalTime = timeManager.elapsedTime;

            totalTimeText.text = timeManager.time;
            numberOfSavesText.text = Loader.SaveAmount.ToString();
            results_Text.text = getRating(totalTime, numberOfSaves);
        }

        public string getRating(float time, int saves)
        {
            print(saves);
            if (time <= sTime && saves <= sSave)
            {
                return "S";
            }
            else if (time <= aTime && saves <= aSave)
            {
                return "A";
            }
            else if (time <= bTime && saves <= bSave)
            {
                return "B";
            }
            else
            {
                return "C";
            }
        }

    }
}

