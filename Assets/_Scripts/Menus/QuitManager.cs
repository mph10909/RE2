using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class QuitManager : MonoBehaviour
    {

        void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuitGame();
                }
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



