using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ResidentEvilClone
{
    public class StartMenu : MonoBehaviour 
    {
        [SerializeField] string[] saveFile;
        [SerializeField] AudioClip startMusic;
        [SerializeField] AudioClip startSound, selectSound, declineSound, cancelSound;
        SaveData data;

        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            SoundManagement.Instance.FadeOutIn(.05f, startMusic);
        }

        public void StartClick()
        {
            Loader.saveAmount = 0;
            StartCoroutine(Start(3, 1));
        }

        public void LoadClick(int file)
        {
            data = SaveManager.GetData(saveFile[file]);
            if (data.sceneInt == 0)
            {
                DeclineSound();
                return;
            }
            Loader.saveAmount = data.saveAmount;
            print(Loader.saveAmount);
            Loader.SaveFile = saveFile[file];
            Loader.Loaded = true;
            StartCoroutine(Start(3, data.sceneInt));

        }

        public void SelectSound()
        {
            SoundManagement.Instance.PlaySound(selectSound);
        }

        public void DeclineSound()
        {
            SoundManagement.Instance.PlaySound(declineSound);
        }

        public void CancelSound()
        {
            SoundManagement.Instance.PlaySound(cancelSound);
        }


        private IEnumerator Start(float time, int scene)
        {
            SoundManagement.Instance.PlaySound(startSound);
            Fader.Instance.FadeOut(time, false);
            SoundManagement.Instance.FadeOut(time);
            yield return new WaitForSeconds(time - 0.1f);
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }



    }
}
