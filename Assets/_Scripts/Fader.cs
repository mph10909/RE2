using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class Fader : MonoBehaviour
    {
        public static Fader Instance;
        [SerializeField] GameObject fade;
        [SerializeField] CanvasRenderer fadeImage;
        [SerializeField] Text fadeText;

        void Awake()
        {
                if (Instance == null)
                {
                    Instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
        }

        public void FadeIn(float fadeTime, bool cursorVisable)
        {
            
            fadeImage.SetAlpha(1);
            fade.SetActive(true);
            StartCoroutine(Fade(0, fadeTime, cursorVisable));
        }

        public void FadeInText(float fadeTime, bool cursorVisible, string text, float delay = 0f)
        {
            fadeImage.SetAlpha(1);
            fade.SetActive(true);
            if (fadeText != null)
            {
                fadeText.text = text;
                fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, 1);
            }
            StartCoroutine(FadeInTextCoroutine(fadeTime, cursorVisible, delay));
        }

        public void FadeOut(float fadeTime, bool cursorVisable)
        {
            Cursor.visible = cursorVisable;
            fadeImage.SetAlpha(0);
            fade.SetActive(true);
            StartCoroutine(Fade(1, fadeTime, cursorVisable));
        }

        private IEnumerator FadeInTextCoroutine(float fadeTime, bool cursorVisible, float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }

            yield return StartCoroutine(Fade(0, fadeTime, cursorVisible));

            if (fadeText != null)
            {
                fadeText.text = "";
            }
        }

        private IEnumerator Fade(float endValue, float duration, bool cursorVisible)
        {
            float time = 0;
            float startValue = fadeImage.GetAlpha();
            float startTextAlpha = fadeText != null ? fadeText.color.a : 1;
            while (time < duration)
            {
                float t = time / duration;
                fadeImage.SetAlpha(Mathf.Lerp(startValue, endValue, t));
                if (fadeText != null)
                {
                    fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b,
                        Mathf.Lerp(startTextAlpha, endValue, t));
                }
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            fadeImage.SetAlpha(endValue);
            if (fadeText != null)
            {
                fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, endValue);
            }
            Cursor.visible = cursorVisible;
            fade.SetActive(false);
            Actions.SetText?.Invoke();
            Actions.FadeTrigger?.Invoke();

        }
    }
}
