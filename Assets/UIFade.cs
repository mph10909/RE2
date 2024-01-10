using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class Fade : MonoBehaviour
    {
        public static Fade Instance { get; private set; }

        public Image fadeImage;
        private bool isFading = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Assume the Image component is attached to the same GameObject
                if (fadeImage == null)
                {
                    Debug.LogError("Fade Image component is not attached to the GameObject.");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DoFade(float targetAlpha, float time, System.Action onComplete = null)
        {
            if (!isFading && fadeImage != null)
            {
                StartCoroutine(FadeRoutine(targetAlpha, time, onComplete));
            }
        }

        public void DoFade(float targetAlpha, float time)
        {
            if (!isFading && fadeImage != null)
            {
                StartCoroutine(FadeRoutine(targetAlpha, time));
            }
        }

        public IEnumerator FadeRoutine(float targetAlpha, float time, System.Action onComplete)
        {
            isFading = true;
            float startAlpha = fadeImage.color.a;
            for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / time)
            {
                Color newColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(startAlpha, targetAlpha, t));
                fadeImage.color = newColor;
                yield return null;
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
            isFading = false;
            onComplete?.Invoke();
        }

        public IEnumerator FadeRoutine(float targetAlpha, float time)
        {
            isFading = true;
            float startAlpha = fadeImage.color.a;
            for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / time)
            {
                Color newColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(startAlpha, targetAlpha, t));
                fadeImage.color = newColor;
                yield return null;
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
            isFading = false;
        }
    }
}
