using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ResidentEvilClone
{
    public class ReturnHome : MonoBehaviour
    {
        public void MainMenu()
        {
            SceneManager.LoadSceneAsync("Start", LoadSceneMode.Single);
        }
    }
}
