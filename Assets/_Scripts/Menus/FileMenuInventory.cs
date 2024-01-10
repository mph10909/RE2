using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{
    public class FileMenuInventory : MonoBehaviour
    {
        public static FileMenuInventory Instance { get; private set; }

        public FileInventory fileInventory;
        [SerializeField]  FileReader displayText;
        [SerializeField]  Text filePage, fileName;
        [SerializeField]  GameObject leftPage, rightPage;
        [SerializeField]  Image fileIcon; 
        public bool flipping;
        int fileEntry = 1;
        int file = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        void OnEnable()
        {
            fileEntry = 1;
            file = 0;
            if (fileInventory.fileList.Count == 0) { NoFiles(); return; }
            SetFile();   
        }

        void NoFiles()
        {
            fileName.text  = "No Files Available";
            filePage.text  = "0/0";
        }

        public void OnClick_Left(InputAction.CallbackContext context)
        {
            if (!this.gameObject.activeSelf || flipping) return;
            if (fileEntry == 1) return;
            if (context.performed)
            {
                leftPage.SetActive(true);
                fileEntry--;
                file--;
                SetFile();
            }
        }

        public void OnClick_Right(InputAction.CallbackContext context)
        {
            if (!this.gameObject.activeSelf || flipping) return;
            if (fileInventory.fileList.Count == 0) return;
            if (context.performed)
            {
                if (fileEntry == fileInventory.fileList.Count) return;
                rightPage.SetActive(true);
                fileEntry++;
                file++;
                SetFile();
            }
        }

        public void OnClick_Enter(InputAction.CallbackContext context)
        {
            if (fileInventory.fileList.Count == 0) return;
            if (!context.performed) return;
            Actions.TextBodySet?.Invoke(TextBody());
        }

        public void OnClick_EnterButton()
        {
            if (fileInventory.fileList.Count == 0) return;
            Actions.TextBodySet?.Invoke(TextBody());
        }

        private void SetFile()
        {
            filePage.text = $"{fileEntry.ToString()}/{fileInventory.fileList.Count.ToString()}";
            fileName.text = fileInventory.fileList[file].title;
            fileIcon.sprite = fileInventory.fileList[file].fileIcon;

        }

        string[] TextBody()
        {
            return fileInventory.fileList[file].body;
        }

        void Flipping()
        {
            if (!flipping) flipping = true;
            else flipping = false;
        }

    }
}
