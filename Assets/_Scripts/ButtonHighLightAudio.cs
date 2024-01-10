using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class ButtonHighLightAudio : MonoBehaviour, IDeselectHandler
    {
        [SerializeField] MenuSounds menuSound;
        [SerializeField] bool notButton;
        
        void OnEnable()
        {
            var button = GetComponent<Button>();
            if(button != null) button.interactable = true;
            //menuSound = MenuSounds.Click;
        }

        public void Disable()
        {
            GetComponent<Button>().interactable = false;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            var button = GetComponent<Button>();
            if (button !=null && button.interactable) SoundManagement.Instance.MenuSound(menuSound);
            if (notButton) SoundManagement.Instance.MenuSound(menuSound);
        }

        public void OnClick()
        {
            SoundManagement.Instance.MenuSound(menuSound);
        }

    }
}
