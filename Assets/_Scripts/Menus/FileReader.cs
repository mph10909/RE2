using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class FileReader : MonoBehaviour
    {
        [HideInInspector] public string[] fileText;
        [SerializeField]  Text currentPage, pageAmount;
        [SerializeField]  Transform textSlotContainer, textSlotTemplate;
        [SerializeField]  GameObject leftArrow, rightArrow, rightExit, leftExit;
        [SerializeField]  AudioClip pageFlip;
        float textSlotCellSizeX = 320;
        float textSlotCellSizeY = 0;
        float slideSpeed = 0.5f;
        int slide = 320;
        int pageInt = 1;
        bool sliding = false;

        void OnEnable()
        {
            Actions.TextBodySet += SetText;
            pageInt = 1;
            SetArrows();
        }

        void OnDisable()
        {
            foreach (Transform child in textSlotContainer)
            {
                if (child == textSlotTemplate) continue;
                Destroy(child.gameObject);
            }
        }
        
        public void OnClick_Left()
        {
            if (textSlotContainer.childCount == 1)return;
            if (!this.gameObject.activeSelf) return;
            if (sliding) return;
                if (pageInt == 1) return;
                SoundManagement.Instance.PlaySound(pageFlip);
                sliding = true;
                pageInt--;
                currentPage.text = pageInt.ToString();
                MoveItems(new Vector3(slide, 0, 0));
        }

        public void OnClick_Right()
        {
            if (textSlotContainer.childCount == 1) return;
            if (!this.gameObject.activeSelf) return;
            if (sliding) return;
                if (pageInt == fileText.Length) return;
                SoundManagement.Instance.PlaySound(pageFlip);
                sliding = true;
                pageInt++;
                currentPage.text = pageInt.ToString();
                MoveItems(new Vector3(-slide, 0, 0));
        }

        void SetTextItems()
        {
            pageAmount.text = fileText.Length.ToString();
            currentPage.text = pageInt.ToString();
            foreach (Transform child in textSlotContainer)
            {
                if (child == textSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;

            foreach (string page in fileText)
            {
                RectTransform textSlotRectTransfrom = Instantiate(textSlotTemplate, textSlotContainer).GetComponent<RectTransform>();
                textSlotRectTransfrom.gameObject.SetActive(true);
                textSlotRectTransfrom.anchoredPosition = new Vector2(x * textSlotCellSizeX, y * textSlotCellSizeY);
                Text pageText = textSlotRectTransfrom.Find("Text").GetComponent<Text>();

                pageText.text = page;

                x++;
            }
        }

        void SetArrows()
        {
            if(textSlotContainer.childCount <= 2)
            {
                leftExit.SetActive(true);
                rightExit.SetActive(false);
                rightArrow.SetActive(false);
                leftArrow.SetActive(false);
            }
            else if(pageInt == 1)
            {
                leftArrow.SetActive(false);
                rightExit.SetActive(false);
                leftExit.SetActive(true);
                rightArrow.SetActive(true);
            }
            else if(pageInt > 1 && pageInt < fileText.Length)
            {
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);
                leftExit.SetActive(false);
                rightExit.SetActive(false);
            }
            else if(pageInt == fileText.Length)
            {
                leftArrow.SetActive(true);
                rightExit.SetActive(true);
                rightArrow.SetActive(false);
                leftExit.SetActive(false);
            }
        }

        void SetText(string[] textBody)
        {
            fileText = textBody;
            SetTextItems();
            SetArrows();
        }

        void MoveItems(Vector3 Direction)
        {
            foreach (Transform child in textSlotContainer)
            {
                if (child == textSlotTemplate) continue;
                Vector3 newPosition = child.localPosition + Direction;
                StartCoroutine(LerpPosition(child, newPosition, slideSpeed));
            }
        }

        IEnumerator LerpPosition(Transform child, Vector3 targetPosition, float duration)
        {
            leftExit.SetActive(false);
            rightExit.SetActive(false);
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            float time = 0;
            Vector3 startPosition = child.localPosition;
            while (time < duration)
            {
                child.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            child.localPosition = targetPosition;
            sliding = false;
            SetArrows();
        }
    }
}
