using UnityEngine;
using UnityEngine.Events;

namespace ResidentEvilClone
{
    public class StorageChest : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject storageMenu;
        [SerializeField] Animator animator;
        [SerializeField] AudioClip openClip;

        public UnityEvent openChest;
        public UnityEvent closeChest;

        const string OPEN = "Open";
        const string CLOSE = "Close";

        public void Interact()
        {
            OpenChest();
        }

        void OpenChest()
        {
            animator.SetTrigger(OPEN);
        }

        void CloseChest()
        {
            animator.SetTrigger(CLOSE);
        }

        void OpenAudio()
        {
            SoundManagement.Instance.PlaySound(openClip);
        }

        void OpenStorageMenu()
        {
            storageMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
