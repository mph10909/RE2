using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    IDeactivate deactivate;

    private void UseItem(Item item){
         
    }

    private void Update(){
        if (Inventory.MenuSeletionUp) return;
        if (Input.GetButton("Submit")){
            if (deactivate != null) deactivate.DeactiveUsable();
        }
    }
    private void OnTriggerEnter(Collider collider) {
        IUsable usable = collider.GetComponent<IUsable>();
        deactivate = collider.GetComponent<IDeactivate>();
        if(usable !=null) usable.ActivateObject();
    }

}        
