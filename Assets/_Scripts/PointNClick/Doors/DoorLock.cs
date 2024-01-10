using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class DoorLock : MonoBehaviour, IComponentSavable
    {
        public bool isLocked;
        public bool isTyperWriter;
        public string isLockedMessage;
        public AudioClip isLockedClip;

        protected BaseDoor door;

        public bool IsLocked { get { return isLocked; } set { isLocked = value; } }

        public virtual void Awake()
        {
            door = GetComponent<BaseDoor>();
        }

        public virtual void TryToUnlock(out bool open)
        {
            UIText.Instance.StartDisplayingText(isLockedMessage, isTyperWriter);

            open = false;
        }

        public virtual void OnTryToUnlockFromExit(out bool openImmediately) { openImmediately = false; }


        public string GetSavableData()
        {
            return isLocked.ToString();
        }

        public void SetFromSaveData(string savedData)
        {
            isLocked = Convert.ToBoolean(savedData);
        }
    }
}
