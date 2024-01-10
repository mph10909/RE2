using UnityEngine;
using UnityEngine.Events;
using ResidentEvilClone;

public class InteractableKeyItem : MonoBehaviour
{
    [SerializeField] private Item.ItemType item;
    [SerializeField] [TextArea(3,5)] string noExtinquisher = "";
    [SerializeField] AudioClip activatedSound;

    [Header("Activated")]
    public UnityEvent Activated;

    public Item GetKeyItem()
    {
        return new Item {itemType = item};
    }

    public void Interact()
    {

        if (CharacterManager.Instance.CheckKeyItem(item))
        {
            CanActivate();
        }
        else
        {
            CantActivate();
        }
        
    }

    void CantActivate()
    {
        UIText.Instance.StartDisplayingText(noExtinquisher, false);
    }

    void CanActivate()
    {
        CharacterManager.Instance.RemoveKeyItem(GetKeyItem());
        SoundManagement.Instance.PlaySound(activatedSound);
        Activated?.Invoke();
    }
}
