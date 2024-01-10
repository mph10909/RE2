using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class TestInteract : MonoBehaviour, IInteractable
    {
        float finalScale = 5.0f;
        float scaleSpeed = 2.0f;
        bool interacted = false;
        float timer = 0.0f;
        bool isScalingUp = true;

        public void Interact()
        {
            if (!interacted)
            {
                interacted = true;
                isScalingUp = true;
            }
        }

        private void Update()
        {
            if (interacted)
            {
                Scale();
            }
        }

        private void Scale()
        {
            timer += Time.deltaTime * scaleSpeed;
            float newScale = 0.0f;
            if (isScalingUp)
            {
                newScale = Mathf.Lerp(1.0f, finalScale, timer);
                if (timer >= 5.0f)
                {
                    timer = 0.0f;
                    isScalingUp = false;
                }
            }
            else
            {
                newScale = Mathf.Lerp(finalScale, 1.0f, timer);
                if (timer >= 5.0f)
                {
                    timer = 0.0f;
                    isScalingUp = true;
                    interacted = false;
                }
            }
            transform.localScale = new Vector3(newScale, newScale, 1.0f);
        }
    }
}
