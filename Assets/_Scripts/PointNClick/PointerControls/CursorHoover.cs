using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHoover : MonoBehaviour
{


    void OnMouseEnter()
    {
        if (Time.timeScale != 1 || !this.enabled) return;
        if (GameStateManager.Instance.CurrentGameState != GameState.GamePlay) return;
        CursorControl.instance.Clickable();
    }
    void OnMouseExit()
    {
        CursorControl.instance.Default();
    }

    public void MouseOver()
    {
        if(!enabled) CursorControl.instance.Default();
    }

}

