using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class SceneFader : MonoBehaviour
    {
        public static SceneFader Instance;
        [SerializeField] GameObject fade;
        [SerializeField] CanvasRenderer fadeImage;


        void Awake()
        {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }

                //fade.SetActive(true);
                //StartCoroutine(FadeIn(targetValue, fadeTime));
        }

        public void FadeIn(float fadeTime)
        {
            fadeImage.SetAlpha(1);
            StartCoroutine(FadeIn(0, fadeTime));
        }

        public void FadeOut(float fadeTime)
        {
            fadeImage.SetAlpha(0);
            StartCoroutine(FadeIn(1, fadeTime));
        }



        private IEnumerator FadeIn(float endValue, float duration)
        {
            float time = 0;
            float startValue = fadeImage.GetAlpha();
            while (time < duration)
            {
                fadeImage.SetAlpha(Mathf.Lerp(startValue, endValue, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            fadeImage.SetAlpha(endValue);
            Cursor.visible = true;
            fade.SetActive(false);
        }
    }
}
