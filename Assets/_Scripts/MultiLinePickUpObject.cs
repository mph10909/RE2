using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PlayerDetection))]
[RequireComponent(typeof(ClickableObject))]
[RequireComponent(typeof(CollectableItem))]
[RequireComponent(typeof(ItemSaveData))]

public class MultiLinePickUpObject : DisplayText, IInteractable
{
    [Header("Text Fields")]
    [SerializeField]string[] text;
    int startingText = 0;
    bool finished;

    [Header("Animation Settings")]
    [SerializeField] bool standingPickUp;
    Animator anim;

    [Header("Inventory and Item Information")]
    PlayerInventory inventory;
    CollectableItem inventoryObject;
    [SerializeField][TextArea(3,5)] string itemTaken = "You have taken"; 
    [SerializeField][TextArea(3,5)] string inventoryFull = "You can not add this to your inventory...";

    [Header("List of Game Objects To Enable")]
    [SerializeField] List<GameObject> gameobjects = new List<GameObject>();

    [Header("Coroutine for Destroying Object")]
    IEnumerator coroutine;

    [Header("Flag to Track if Object has been Picked Up")]
    bool objectPickedup = false;

    [Header("Constants for Animation Triggers")]
    const string STANDING = "Standing", PICKUP = "ItemPickUp", PICKEDUP = "ItemPickedUp";

    [Header("Reference to Character Game Object")]
    GameObject character;

    bool active;


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

    public override void Awake()
    {
        base.Awake();
        inventoryObject = GetComponent<CollectableItem>();
        coroutine = WaitAndDestroy(0.1f);
        gameObject.tag = "Collectible";
        Actions.CharacterSwap += SetCharacter;
    }

    void OnDisable()
    {
        Actions.CharacterSwap -= SetCharacter;
    }

    void SetCharacter(GameObject player)
    {
        Character = player;
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

        if (startingText != 0) return;
        active = true;
        finished = false;
        TextDisplay(text[startingText]);
    }

    void Update()
    {
        if (!active) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!objectPickedup)
            {
            LastLine();
            NextTextLine();
            }
        }
        if (Input.GetMouseButtonDown(1)) Finished();
    }

    void LastLine()
    {
        if (startingText == text.Length - 1)
        {
            PickUp();
            return;
        }
    }

    void NextTextLine()
    {
        if (finished) return;
        startingText++;
        TextDisplay(text[startingText]);
    }

    void PickUp()
    {
        finished = true;
        anim.SetTrigger(PICKUP);
        if (standingPickUp) anim.SetTrigger(STANDING);
        TextClear();
        TextDisplay(itemTaken + " " + "<color=green>" + inventoryObject.item.GetText() + "</color>");
        objectPickedup = true;
    }

    private void ActivatveFinishedItems()
    {
        foreach (GameObject interactables in gameobjects)
        {
            Actions.PathIsFinished?.Invoke();
            print("Interactable " + interactables.name);
            var interacting = interactables.GetComponent<IInteractable>();
            if (interacting != null) interacting.Interact();
        }
    }

    void Finished()
    {
        finished = true;
        Time.timeScale = 1;
        TextClear();
        startingText = 0;
        active = false;
        return;
    }

    void ItemToInventory()
    {
        if (inventoryObject.tag == "Collectible")
        {
            inventory.inventory.AddItem(inventoryObject.GetItem());
            GameStateManager.Instance.SetState(GameState.GamePlay);
        }
    }

    void DestroyThisObject()
    {
        if (objectPickedup)
        {
            Actions.TextClear -= DestroyThisObject;
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        SoundManagement.Instance.MenuSound(MenuSounds.Accept);
        yield return new WaitForSecondsRealtime(waitTime);
        anim.SetTrigger(PICKEDUP);
        ItemToInventory();
        ActivatveFinishedItems();
        this.gameObject.SetActive(false);
        CursorControl.instance.Default();
    }
}
