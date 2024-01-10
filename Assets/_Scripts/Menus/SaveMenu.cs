using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class SaveMenu : MonoBehaviour
    {
        [SerializeField] GameObject saveMenu;
        [SerializeField] AudioClip startSound, selectSound, declineSound, cancelSound;

        public void OnClick_Cancel()
        {
            if (saveMenu.activeSelf == false) return;
            SoundManagement.Instance.PlaySound(cancelSound);
            StartCoroutine(Cancel(0.15f, 1));
        }

        IEnumerator Cancel(float time, float timeScale)
        {
            Fader.Instance.FadeOut(time, true);
            yield return new WaitForSecondsRealtime(time);
            Time.timeScale = timeScale;
            saveMenu.SetActive(false);
        }



    }
}
