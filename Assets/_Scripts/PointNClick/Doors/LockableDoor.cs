using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;
//using UnityEngine.UI;

public enum LockedDoorAudio : int
{
    Locked,
    UnLocked
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BooleanSaveData))]
public class LockableDoor : Door, IInteractable
{

    [Header("Door Settings")]
    [SerializeField] private bool locked = true;
    [SerializeField] private bool lockedNoKey = false;
    [SerializeField] private bool destroyKeyItem;
    [SerializeField] private Item.ItemType keyItem;

    [Header("Second Door To Unlock")]
    [SerializeField] private LockableDoor secondDoor;

    [Header("Door Audio Clips")]
    [SerializeField] private List<DoorAudio> doorClips = new List<DoorAudio>();

    [Header("Locked Door Text")]
    [SerializeField] [TextArea(3, 5)] private string lockedText = "The door is locked.";

    [Header("Unlocking Door Text")]
    [SerializeField] [TextArea(3, 5)] private string unlockingText = "You've used the";

    [Header("Doors That Use Same Key")]
    [SerializeField] LockableDoor[] sharedKeyDoors;

    public bool Locked { get { return locked; } set { locked = value; } }
    public Item.ItemType Key { get { return keyItem; } }


    [System.Serializable]
    public struct DoorAudio
    {

#if UNITY_EDITOR
        [HideInInspector]
        public string name;
#endif
        public AudioClip soundClip;
    }

    public Item.ItemType GetKeyItem()
    {
        return keyItem;
    }

    public override void Interact()
    {
        HandleLockedDoorInteraction();
    }

    public void HandleLockedDoorInteraction()
    {
        if (lockedNoKey)
        {
            LockedNoKey();
        }
        else if (locked)
        {
            LockedDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public virtual void UnlockDoor(Item key)
    {
        if ((doorClips[(int)LockedDoorAudio.UnLocked].soundClip != null))
        {
            SoundManagement.Instance.PlaySound(doorClips[(int)LockedDoorAudio.UnLocked].soundClip);
        }


        TextDisplay(unlockingText + "\n" + "<color=green>" + key.GetText() + "</color>");
         
        locked = false;

        if (secondDoor != null)
        {
            secondDoor.locked = false;
        }


        if (AllDoorsUnlocked() && destroyKeyItem)
        {
            print("All Doors Unlocked With This Key");

            AllDoorsUnlockedText("This item is no longer needed");
        }
    }

    public void AllDoorsUnlockedText(string displayText)
    {
        Inventory.inventory.RemoveItemType(keyItem);
        _displayText = displayText;
        Actions.TextClear -= TextClear;
        Actions.TextClear += NextLine;
    }

    public bool AllDoorsUnlocked()
    {
        if (sharedKeyDoors.Length == 0)
        {
            return true;
        }

        bool allUnlocked = true;

        foreach (LockableDoor lockedDoor in sharedKeyDoors)
        {
            if (lockedDoor.Locked)
            {
                allUnlocked = false;
                break;
            }
        }

        return allUnlocked;
    }

    public void LockedNoKey()
    {
        if ((doorClips[(int)LockedDoorAudio.UnLocked].soundClip != null)) {
            SoundManagement.Instance.PlaySound(doorClips[(int)LockedDoorAudio.UnLocked].soundClip);}
        TextDisplay(unlockingText);
        locked = false;
        lockedNoKey = false;
        if (secondDoor != null)
            {
                secondDoor.locked = false;
            }
    }

    public virtual void LockedDoor()
    {
        if (locked)
        {

            foreach (Item item in Inventory.inventory.GetItemList())
            {
                if (item.itemType == GetKeyItem())
                {
                    UnlockDoor(item);
                    return;
                }
            }
            SoundManagement.Instance.PlaySound(doorClips[(int)LockedDoorAudio.Locked].soundClip);
            TextDisplay(lockedText);
        }
    }

    #region SoundEffectValidate
#if UNITY_EDITOR
    void Reset() { OnValidate(); }
    void OnValidate()
    {
        var ammoNames = System.Enum.GetNames(typeof(LockedDoorAudio));
        var inventory = new List<DoorAudio>(ammoNames.Length);
        for (int i = 0; i < ammoNames.Length; i++)
        {
            var existing = doorClips.Find(
                (entry) => { return entry.name == ammoNames[i]; });
            existing.name = ammoNames[i];
            inventory.Add(existing);
        }
        doorClips = inventory;

    }
#endif
    #endregion
}