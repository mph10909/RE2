using UnityEngine;
using ResidentEvilClone;
using UnityEngine.Events;

[System.Serializable]
public class ClickableData
{
    public Transform inspectionLocation;
    public ClickableAction clickData;
}

[System.Serializable]
public class OnInteractionEvent : UnityEvent<IInteractor> { }

public abstract class Clickable : CursorHoover
{
    public ClickableData clickableData;
    public UnityEvent<IInteractor> Interact;

}
