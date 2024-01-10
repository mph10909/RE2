using UnityEngine;
using UnityEngine.Events;
using ResidentEvilClone;
using System;

public class PlayerDetectDisables : BaseMessage
{
    public GameObject ThisGameObject;
}

public class PlayerDetect : Clickable
{
    private Collider col;
    PlayerDetectDisables thisGameObject = new PlayerDetectDisables();

    void Start()
    {
        thisGameObject.ThisGameObject = this.gameObject;
        col = gameObject.FindCollider();
    }

    void OnDisable()
    {
            MessageBuffer<PlayerDetectDisables>.Dispatch(thisGameObject);
    }

    public void EnableClickable()
    {
        this.gameObject.SetActive(true);
    }

    // Method to disable the clickable GameObject
    public void DisableClickable()
    {
        this.gameObject.SetActive(false);
    }

}

