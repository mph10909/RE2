using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuScript : MonoBehaviour
{
    [SerializeField] GameObject MainMenu, MapMenu;
    [SerializeField] GameObject[] ExitButtons;
    MenuController menuController;
    [SerializeField] bool ExitMapHighlighted;
    
    void Start()
    {
        ExitMapHighlighted = false;
    }
    void OnEnable() 
    {
        ExitButtons[0].GetComponent<TextMesh>().color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        MapController();
        if(ExitMapHighlighted && Input.GetKeyDown(KeyCode.Return)) MapMenuClose();
        if(Input.GetKeyDown(KeyCode.Escape)) MapMenuClose();
    }
    void MapMenuClose()
    {
      MenuController.MapMenuActive  = false;
      menuController.MenuSelected(); 
      MapMenu.SetActive(false);
    }

    void MapController()
    {
        //if(ExitButtons != null) return;   
        if(Input.GetKeyDown(KeyCode.D)){ExitButtons[0].GetComponent<TextMesh>().color = Color.white; ExitMapHighlighted = true;}
        if(Input.GetKeyDown(KeyCode.A)){ExitButtons[0].GetComponent<TextMesh>().color = Color.gray; ExitMapHighlighted = false;}
    }
}
