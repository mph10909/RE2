using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class FilePickUp : MonoBehaviour
    {

        [SerializeField][TextArea(3,5)] string text = "You have taken"; 
        [SerializeField] FileData fileData;

        public bool typerwriter;

        IEnumerator coroutine;

        void OnEnable()
        {
            coroutine = WaitAndDestroy(0.1f);
        }


        public void Interact()
        {
            PauseController.Instance.Pause();
            PickUp();
        }

        void PickUp()
        {
            MessageBuffer<UITextComplete>.Subscribe(DestroyThisObject);
            UIText.Instance.StartDisplayingText(text + "\n" + "<color=green>" + fileData.title + "</color>", false);
        }

        void DestroyThisObject(UITextComplete msg)
        {
            StartCoroutine(coroutine);
        }

        void ItemToInventory()
        {
            FileMenuInventory.Instance.fileInventory.AddFileItem(fileData);
            GameStateManager.Instance.SetState(GameState.GamePlay);
        }

        private IEnumerator WaitAndDestroy(float waitTime)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            yield return new WaitForSecondsRealtime(waitTime);
            ItemToInventory();
            this.gameObject.SetActive(false);
        }
    }
}
