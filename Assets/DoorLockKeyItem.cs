using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class DoorLockKeyItem : DoorLock
    {
        [SerializeField][TextArea(3,5)] string unLockedMessage;
        [SerializeField][TextArea(3,5)] string otherSideLockedMessage;
        [SerializeField] AudioClip unLockedSound;

        [SerializeField] bool destroyItem;
        [SerializeField] bool useBothSides;

        public Item.ItemType keyItem;

        // Start is called before the first frame update
        public override void Awake()
        {
            base.Awake();
        }

        public override void TryToUnlock(out bool open)
        {
            if (!TryUnlock())
            {
                if (isLockedClip)
                {
                    SoundManagement.Instance.PlaySound(isLockedClip);
                }
                if(isLockedMessage != null)
                {
                    UIText.Instance.StartDisplayingText(isLockedMessage, isTyperWriter);
                }
               
            }

            open = false;
        }

        // Update is called once per frame
        public bool TryUnlock()
        {
            if (CharacterManagement.Instance.CheckKeyItem(keyItem))
            {
                if (destroyItem)
                    CharacterManagement.Instance.RemoveKeyItem(keyItem);

                if (useBothSides)
                {
                    BaseDoor baseDoor = door as BaseDoor;
                    if (baseDoor)
                    {
                        var exitLock = baseDoor.Exit.GetComponent<DoorLockKeyItem>();
                        if(exitLock && exitLock.IsLocked && exitLock.keyItem == keyItem)
                        {
                            exitLock.IsLocked = false;
                        }
                    }
                }

                IsLocked = false;

                if (unLockedSound)
                    SoundManagement.Instance.PlaySound(unLockedSound);
                if (unLockedMessage != null)
                    UIText.Instance.StartDisplayingText(unLockedMessage + "\n" + "<color=green>" + Item.GetText(keyItem) + "</color>", isTyperWriter);

                return true;
            }
            return false;
        }
    }
}
