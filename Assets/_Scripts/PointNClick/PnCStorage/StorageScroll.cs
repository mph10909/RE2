using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class StorageScroll : MonoBehaviour
    {
        [SerializeField] float speed = 0.5f;   
        [SerializeField] AudioClip click; 
        [SerializeField] Button[] buttons;

        Transform[] rtChildren;
        Transform rectTransform, firstTransform, lastTransform;
        
        float top, bottom, spacing;
        bool moving;

        void OnDisable()
        {
            Enable(false);
        }

        void Start()
        {
            ItemStorageManager.Scrolling += Enable;
            rectTransform = GetComponent<Transform>();
            rtChildren = new Transform[rectTransform.childCount];
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                rtChildren[i] = rectTransform.GetChild(i) as Transform;
            }

            firstTransform = rectTransform.GetChild(0);
            lastTransform = rectTransform.GetChild(rectTransform.childCount-1);
            top = lastTransform.position.y;
            bottom = firstTransform.position.y;

            spacing = (lastTransform.position.y - firstTransform.position.y) / (rectTransform.childCount - 1);
            Enable(false);
        }

        private void Enable(bool enabled)
        {
            this.enabled = enabled;
            foreach(Button button in buttons)
            {
                button.GetComponent<Button>().enabled = enabled;
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveItemsUp();
            }
            if (Input.GetKey(KeyCode.S))
            {
                MoveItemsDown();
            }

        }

        private void MoveItemsUp()
        {
            if (moving) return;
            SoundManagement.Instance.PlaySound(click);
            foreach(Transform item in rtChildren)
            {
                StartCoroutine(LerpPosition(item, new Vector2(item.position.x, item.position.y + spacing), speed));
            }
        }

        private void MoveItemsDown()
        {
            if (moving) return;
            SoundManagement.Instance.PlaySound(click);
            foreach (Transform item in rtChildren)
            {
                StartCoroutine(LerpPosition(item, new Vector2(item.position.x, item.position.y - spacing), speed));
            }
        }

        private void CheckItemPosition(Transform item)
        {
            if(item.position.y > top + 1)
            {
                item.position = new Vector2(item.position.x, bottom);
                return;
            }
            if (item.position.y < bottom -1)
            {
                item.position = new Vector2(item.position.x, top);
                return;
            }
        }

        IEnumerator LerpPosition(Transform item, Vector2 targetPosition, float duration)
        {
            moving = true;
            float time = 0;
            Vector2 startPosition = item.position;
            while (time < duration)
            {
                item.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            item.position = targetPosition;
            moving = false;
            CheckItemPosition(item);
        }

    }
}
