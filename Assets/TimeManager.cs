using System;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class TimeManager : MonoBehaviour, IComponentSavable
    {
        public float elapsedTime; // the elapsed time since the game started
        private bool isPaused; // whether the time is currently paused
        public string time;

        void Awake()
        {
            Actions.PauseTime += PauseTime;
            Actions.ResumeTime += ResumeTime;

        }

        private void Update()
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;
                UpdateTimeText();
            }
        }

        private void UpdateTimeText()
        {
            int hours = Mathf.FloorToInt(elapsedTime / 3600f);
            int minutes = Mathf.FloorToInt((elapsedTime - hours * 3600f) / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime - hours * 3600f - minutes * 60f);

            /*timeText.text*/
            time = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            //time = timeText.text.ToString();
        }

        public void PauseTime()
        {
            isPaused = true;
        }

        public void ResumeTime()
        {
            isPaused = false;
        }

        public string GetSavableData()
        {
            return elapsedTime.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            elapsedTime = (float)Convert.ToDouble(savedData);
        }
    }
}
