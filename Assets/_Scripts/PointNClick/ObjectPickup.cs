using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [RequireComponent(typeof(SavableObject))]

    public class ObjectPickup : MonoBehaviour
    {
        [Header("Item")]
        public Item item;

        [Header("Item Text")]
        [TextArea(3, 5)] public string text = "You have taken";
        [TextArea(3, 5)] public string inventoryFull = "You can not add this to your inventory...";
        

        IEnumerator coroutine;

        public bool typerwriter;


        public virtual void OnEnable()
        {
            coroutine = WaitAndDestroy(0.1f);
        }

        public virtual void Interact()
        {
            PauseController.Instance.Pause();

            if (CheckAndHandleInventoryFull())
            {
                return;
            }

            PickUp();
        }

        public virtual void PickUp()
        {
            MessageBuffer<UITextComplete>.Subscribe(DestroyThisObject);
            UIText.Instance.StartDisplayingText(text + "\n" + "<color=green>" + item.GetText() + "</color>", typerwriter);
        }

        public virtual void DestroyThisObject(UITextComplete msg)
        {
            MessageBuffer<UITextComplete>.Unsubscribe(DestroyThisObject);
            StartCoroutine(coroutine);
        }

        public void ItemToInventory()
        {
            CharacterManagement.Instance.AddToCurrentInventory(item);
            PauseController.Instance.Resume();
        }

        public virtual IEnumerator WaitAndDestroy(float waitTime)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            yield return new WaitForSecondsRealtime(waitTime);
            ItemToInventory();
            this.gameObject.SetActive(false);
            CursorControl.instance.Default();
        }

        public bool CheckAndHandleInventoryFull()
        {
            if (CharacterManagement.Instance.CheckCurrentInventoryCount())
            {
                SoundManagement.Instance.MenuSound(MenuSounds.Decline);
                UIText.Instance.StartDisplayingText(inventoryFull, typerwriter);
                return true; // Indicates inventory is full
            }
            return false; // Indicates inventory has space
        }

    }
}
