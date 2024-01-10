using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{

    public class HiddenObjectPickup : ObjectPickup, ISavableObjectState
    {
        [Header("Text To Display If Item Aquired")]
        [SerializeField][TextArea(3,5)] string itemPickedUpText = "";

        IEnumerator     coroutine;

        bool objectPickedup = false;

        public bool ObjectPickedup { get { return objectPickedup; } set { objectPickedup = value; } }

        public override void Interact()
        {
            PauseController.Instance.Pause();

            if (objectPickedup)
            {
                UIText.Instance.StartDisplayingText(itemPickedUpText, typerwriter);
                return;
            }

            if (CheckAndHandleInventoryFull())
            {
                return;
            }

            PickUp();
        }

        public override void PickUp()
        {
            base.PickUp();
            objectPickedup = true;
        }

        void DestroyThisObject()
        {
            if (objectPickedup)
            {
                StartCoroutine(coroutine);
            }
        }

        public override IEnumerator WaitAndDestroy(float waitTime)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            yield return new WaitForSecondsRealtime(waitTime);
            ItemToInventory();
        }

        public string GetSavableData()
        {
            return objectPickedup.ToString();
        }

        public virtual void SetFromSaveData(string savedData)
        {
            objectPickedup = Convert.ToBoolean(savedData);

            if (objectPickedup)
            {

            }
        }
    }
}