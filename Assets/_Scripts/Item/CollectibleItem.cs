using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour, IUsable, IDeactivate
{
    [SerializeField]
    GameObject _mainCamera;
    private InventoryTest inventory;
    private ItemWorld itemWorld;
    private Quaternion cameraLocation1, cameraLocation2;
    private Vector3 ItemScaleSize = new Vector3(0.5f, 0.5f, 0.5f);
    private bool itemCanGrow;
    //private bool interactingWithItem;


    void Start()
    {
        _mainCamera = GameObject.FindWithTag("MainCamera");
        inventory = UI_inventory.inventory;
        itemWorld = GetComponent<ItemWorld>(); 
    }


    void Update()
    {   
        if(_mainCamera == null) _mainCamera = GameObject.FindWithTag("MainCamera");
        if (!Inventory.gameIsPaused) return;
        if (itemCanGrow) { this.transform.localScale = Vector3.Lerp(this.transform.localScale, ItemScaleSize, Time.unscaledDeltaTime * 2); }
    }

    public void DeactiveUsable()
    {
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            TextDisplay.TEXT_DISABLER();
            //Tween.PickedUpItem(_mainCamera, cameraLocation2, cameraLocation1, 0.5f, () => itemPickedUp());
        }
    }

    public void ActivateObject()
    {
        cameraLocation1 = _mainCamera.transform.rotation;
        //interactingWithItem = true;
        SoundManager.PlaySound(SoundManager.Sound.menuEnter);
        itemWorld.initializeTextItem();
        Inventory.PAUSE();
        //Tween.Rotation(_mainCamera, this.gameObject, 1.0f, ()=> PickingObjectUp());
    }

    private void PickingObjectUp()
    {
        _mainCamera.transform.rotation = TweenRotate.lookRotation;
        cameraLocation2 = _mainCamera.transform.rotation;
        Destroy(_mainCamera.GetComponent<TweenRotate>());
        this.transform.position = _mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2.5f, Screen.height / 3, 3));
        this.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        this.transform.rotation = Quaternion.Euler(_mainCamera.transform.eulerAngles.x, _mainCamera.transform.eulerAngles.y, _mainCamera.transform.eulerAngles.z * -1);
        itemCanGrow = true;
    }

    private void itemPickedUp()
    {
        Destroy(_mainCamera.GetComponent<itemPickedUp>());
        itemCanGrow = false;
        ComponentController.ItemPickUpActive = false;
        GetComponent<Collider>().enabled = false;
        //interactingWithItem = false;
        SoundManager.PlaySound(SoundManager.Sound.menuExit);
        Inventory.UNPAUSE();
        if (itemWorld.tag == "Item/Item") inventory.AddItem(itemWorld.GetItem());
        if (itemWorld != null) Destroy(this.gameObject);
    }
}
