using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] Vector2[] resolutions2;
        [SerializeField] int currentResolutionIndex = 0;

        public Dropdown resolutionDropDown;
        public Toggle fullscreenToggle;

        [SerializeField] GameObject audioController;

        void Start()
        {
            SetResolutionDropDown();
        }

        void OnEnable()
        {
            SetResolutionDropDown();
        }


        private void SetResolutionDropDown()
        {
            if (Screen.fullScreen)
            {
                fullscreenToggle.isOn = true;
            }

            resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();
            for (int i = 0; i < resolutions2.Length; i++)
            {
                string option = resolutions2[i].x + "x" + resolutions2[i].y;
                options.Add(option);

                if (Mathf.RoundToInt(resolutions2[i].x) == Screen.width && Mathf.RoundToInt(resolutions2[i].y) == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropDown.AddOptions(options);
            resolutionDropDown.value = currentResolutionIndex;
            resolutionDropDown.RefreshShownValue();
        }

        public void SetResolution(int resoutionIndex)
        {
            int x = Mathf.RoundToInt(resolutions2[resolutionDropDown.value].x);
            int y = Mathf.RoundToInt(resolutions2[resolutionDropDown.value].y);
            Screen.SetResolution(x, y, Screen.fullScreen);
            audioController.SetActive(true);
        }

        public void ClickedResolution()
        {
            if (audioController.activeSelf)
            {
                audioController.SetActive(false);
            }
            else
            {
                audioController.SetActive(true);
            }
        }

        public void SetFullScreen(bool isFullScreen)
        {
            fullscreenToggle.Select();
            
            Screen.fullScreen = isFullScreen;
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

    }
}
