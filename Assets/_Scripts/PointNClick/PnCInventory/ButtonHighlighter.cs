using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class ButtonHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] Transform surround;
        [SerializeField] Button thisButton;

        void Awake()
        {
            thisButton = GetComponent<Button>();
            surround   = transform.Find("Surround");
        }

        public void OnPointerEnter(PointerEventData eventdata)
        {
            if (!thisButton.enabled) return;
            Highlighted();
        }

        public void OnPointerExit(PointerEventData eventdata)
        {
            if (!thisButton.enabled) return;
            Deselected();
        }

        public void OnSelect(BaseEventData eventdata)
        {
            //if (!thisButton.enabled) return;
            //Highlighted();
        }

        public void OnDeselect(BaseEventData eventdata)
        {
            //if (!thisButton.enabled) return;
            //Deselected();
        }

        void Highlighted()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Click);
            surround.gameObject.SetActive(true);
        }

        void Deselected()
        {
            surround.gameObject.SetActive(false);
        }
    }
}
