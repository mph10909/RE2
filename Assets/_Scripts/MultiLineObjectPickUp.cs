using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;
using UnityEngine.Events;
using System;

namespace ResidentEvilClone
{
    public class MultiLineObjectPickup : ObjectPickup, ISavableObjectState
    {
        [Header("TextString Fields")]
        [SerializeField]string[] textString;

        [Header("Coroutine for Destroying Object")]
        IEnumerator coroutine;

        [Header("FinishedPickupActions")]
        public UnityEvent FinishedPickup;

        public void Awake()
        {
            coroutine = WaitAndDestroy(0.1f);
        }

        public override void Interact()
        {
            PauseController.Instance.Pause();

            if (CheckAndHandleInventoryFull())
            {
                return;
            }

            MessageBuffer<UITextComplete>.Subscribe(PickUp);
            UIText.Instance.StartDisplayingText(textString, typerwriter);
        }

        public void PickUp(UITextComplete msg)
        {
            
            MessageBuffer<UITextComplete>.Unsubscribe(PickUp);
            base.PickUp();
        }

        void DestroyThisObject()
        {
                StartCoroutine(coroutine);
        }

        public override IEnumerator WaitAndDestroy(float waitTime)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            yield return new WaitForSecondsRealtime(waitTime);
            ItemToInventory();
            FinishedPickup?.Invoke();
            this.gameObject.SetActive(false);
            CursorControl.instance.Default();
        }

        public string GetSavableData()
        {
            return this.gameObject.activeSelf.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            bool isActive;
            isActive = Convert.ToBoolean(savedData);
            this.gameObject.SetActive(isActive);
        }
    }
}