using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject[] Untagged;
    [SerializeField] Material[] MapMaterials;
    MenuController menuController;
    void Start()
    {   
        menuController = MainMenu.GetComponent<MenuController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(MenuController.MapMenuActive)
            {Untagged = GameObject.FindGameObjectsWithTag("EmptyMap");                                            
            foreach(GameObject renderer in Untagged) renderer.GetComponent<Renderer>().enabled = false; return;}
            foreach(GameObject renderer in Untagged) renderer.GetComponent<Renderer>().enabled = true;
    
        
    }


}
