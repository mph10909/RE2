using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PickUpFile : DisplayText, IInteractable
    {

        [SerializeField][TextArea(3,5)] string text = "You have taken"; 
        [SerializeField] bool standingPickUp;
        //[SerializeField] FileItem fileItem;
        [SerializeField] FileData fileData;
        [SerializeField] FileMenuInventory inventory;

        Animator        anim;
        GameObject      character;

        IEnumerator     coroutine;

        bool objectPickedup = false;

        const string STANDING = "Standing", PICKUP = "ItemPickUp", PICKEDUP = "ItemPickedUp";

        private void OnValidate()
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<FileMenuInventory>(true);
            }
        }

        void OnEnable()
            {
               coroutine = WaitAndDestroy(0.1f);
               Actions.CharacterSwap += SetItem;
            }

        void OnDisable()
            {
               Actions.CharacterSwap -= SetItem;
            }   

        void SetItem(GameObject player)
            {    
                character = player;
                anim = player.GetComponentInChildren<Animator>();
             }

        public void Interact()
            {
                Time.timeScale = 0;
                Actions.TextClear += DestroyThisObject;
                PickUp();
            }

        void PickUp()
           {
                anim.SetTrigger(PICKUP);
                if (standingPickUp) anim.SetTrigger(STANDING);
                TextDisplay(text + "\n" + "<color=green>" + fileData.title + "</color>");
                objectPickedup = true;
            }

        void DestroyThisObject()
            {
                if (objectPickedup)
                    {
                        Actions.TextClear -= DestroyThisObject;
                        StartCoroutine(coroutine);
                    }
            }

        void ItemToInventory()
            {
                inventory.fileInventory.AddFileItem(fileData);
                //inventory.fileInventory.AddItem(fileItem);
                GameStateManager.Instance.SetState(GameState.GamePlay);
            }

        private IEnumerator WaitAndDestroy(float waitTime)
            {
                SoundManagement.Instance.MenuSound(MenuSounds.Accept);
                yield return new WaitForSecondsRealtime(waitTime);
                anim.SetTrigger(PICKEDUP);
                ItemToInventory();
                this.gameObject.SetActive(false);
            }
    }
}
