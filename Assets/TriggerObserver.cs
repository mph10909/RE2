using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnTriggerAction : UnityEvent<Collider> { }

public class TriggerObserver : MonoBehaviour
{

    public UnityEvent TriggerEnter;
    public UnityEvent TriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke();
    }

}

