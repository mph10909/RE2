using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class ButtonFader : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] Color startColor;
        [SerializeField] Color endColor;
        [SerializeField] Text text;
        [SerializeField] [Range(0.1f , 5)] float speed = 2;
        
        void OnEnable()
        {
            if(this.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                StartCoroutine(FadeOut(endColor, speed));
            }
        }

        public void OnSelect(BaseEventData eventdata)
        {
            StartCoroutine(FadeOut(endColor, speed));
        }
        public void OnDeselect(BaseEventData eventdata)
        {
            StopAllCoroutines();
            text.color = startColor;
        }


        IEnumerator FadeOut(Color endValue, float duration)
        { 
        float time = 0;
            Color startValue = startColor;
            while (time < speed)
            {
                text.color = Color.Lerp(startValue, endValue, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            text.color = endValue;
            StartCoroutine(FadeIn(startColor, speed));
        }

        IEnumerator FadeIn(Color endValue, float duration)
        {
            float time = 0;
            Color startValue = text.color;
            while (time < speed)
            {
                text.color = Color.Lerp(startValue, endValue, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            text.color = endValue;
            StartCoroutine(FadeOut(endColor, speed));
        }


    }
}
