using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerDetection))]
[RequireComponent(typeof(ClickableObject))]
[RequireComponent(typeof(CollectableItem))]
[RequireComponent(typeof(ItemSaveData))]

public class PickUpObject : DisplayText, IInteractable
{
    [SerializeField][TextArea(3,5)] string text = "You have taken"; 
    [SerializeField][TextArea(3,5)] string inventoryFull = "You can not add this to your inventory...";
    [SerializeField] bool standingPickUp;

    [SerializeField] PlayerInventory inventory;
    [SerializeField] Animator        anim;
    CollectableItem  inventoryObject;
    private GameObject character;

    private IEnumerator coroutine;

    bool objectPickedup = false;

    const string STANDING = "Standing", PICKUP = "ItemPickUp", PICKEDUP = "ItemPickedUp";
    [SerializeField] bool typerwriter;

    void OnValidate()
    {
        inventoryObject = GetComponent<CollectableItem>();
    }

    public GameObject Character
    {
        set
        {
            character = value;
            inventory = character.GetComponent<PlayerInventory>();
            anim = character.GetComponentInChildren<Animator>();
        }
    }

    void OnEnable()
    {
        inventoryObject = GetComponent<CollectableItem>();
        coroutine = WaitAndDestroy(0.1f);
        gameObject.tag = "Collectible";
        Actions.CharacterSwap += SetItem;
    }

    void OnDisable()
    {
        Actions.CharacterSwap -= SetItem;
    }

    void SetItem(GameObject player)
    {    
        Character = player;
        //anim = player.GetComponentInChildren<Animator>();
        //inventory = player.GetComponent<PlayerInventory>();
    }

    public void Interact()
    {
        Time.timeScale = 0;
        Actions.TextClear += DestroyThisObject;

        if (inventory.inventory.itemList.Count >= 8)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Decline);
            TextDisplay(inventoryFull);
            return;
        }
        PickUp();
    }

    void PickUp()
    {
        anim.SetTrigger(PICKUP);
        if (standingPickUp) anim.SetTrigger(STANDING);
        TextDisplay(text + "\n" + "<color=green>" + inventoryObject.item.GetText() + "</color>");
        //UIText.Instance.StartDisplayingText(text + "\n" + "<color=green>" + inventoryObject.item.GetText() + "</color>", typerwriter);
        objectPickedup = true;
    }

    void DestroyThisObject()
    {
        if (objectPickedup)
        {
            print("WTF");
            Actions.TextClear -= DestroyThisObject;
            StartCoroutine(coroutine);
        }
    }

    void ItemToInventory()
    {
        if (inventoryObject.tag == "Collectible")
        {
            inventory.inventory.AddItem(inventoryObject.GetItem());
            GameStateManager.Instance.SetState(GameState.GamePlay);
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        SoundManagement.Instance.MenuSound(MenuSounds.Accept);
        yield return new WaitForSecondsRealtime(waitTime);
        anim.SetTrigger(PICKEDUP);
        ItemToInventory();
        this.gameObject.SetActive(false);
        CursorControl.instance.Default();
    }
}
