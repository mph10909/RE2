using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ResidentEvilClone
{
    public class SceneSwapper : MonoBehaviour
    {
        [System.Serializable]
        public class SceneButtonPair
        {
            public string buttonName;  // The name of the button for reference in the editor
            public SceneReference sceneToLoad;
        }

        public void SwapSceneFromButton(SceneReference sceneReference)
        {
            StartCoroutine(TransitionToScene(sceneReference.sceneName));
        }


        IEnumerator TransitionToScene(string sceneName)
        {
            // Capture state of all savable objects
            ObjectStateManager.Instance.CaptureStates();

            // Start loading the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            SceneManager.sceneLoaded += OnSceneLoaded;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ObjectStateManager.Instance.ApplyStates();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
