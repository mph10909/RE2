using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This class implements the IInteractable interface, allowing the object it is attached to be interacted with
// by the player. The interaction will add the tutorial item to the player's inventory, display a pickup text message,
// and deactivate the object in the game world.
public class TutorialItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject pressToPickUp;
    
    AudioSource audioSource;
    [SerializeField]AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        pressToPickUp.SetActive(true);
    }

    void OnTriggerExit(Collider collider)
    {
        pressToPickUp.SetActive(false);
    }
    //[SerializeField]TutorialItem item; // The tutorial item that will be added to the player's inventory   
    //[SerializeField]string pickUpText; // The text message to display when the player interacts with the object

    //// Property for accessing the pickup text message
    //public string PickUpText
    //{
    //    get { return pickUpText; }
    //}

    //// Property for accessing the tutorial item
    //public TutorialItem Item
    //{
    //    get { return item; }
    //}

    // Implementation of the Interact method from the IInteractable interface
    public void Interact()
    {
        // Invokes the TextMessage delegate with the pickup text message
        //TutorialActionMediator.TextMessage?.Invoke(this.PickUpText);

        // Invokes the ItemToInventory delegate with the tutorial item
        //TutorialActionMediator.ItemToInventory?.Invoke(this.Item);

        // Deactivates the object in the game world
        audioSource.PlayOneShot(audioClip);
        pressToPickUp.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
