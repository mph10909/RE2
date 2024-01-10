using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : CursorHoover
{
    [SerializeField] int Distance = 25;
    CharacterManager charManager;
    [SerializeField]GameObject character;
    Collider col;
    ClickableObject clickableObject;

    public GameObject Character
    {
        set { character = value; }
    }

    public ClickableObject Clickable { get { return clickableObject; } set { clickableObject = value; } }

    void OnEnable()
    {
        Actions.CharacterSwap += SetCharacter;
    }

    void OnDisable()
    {
        Actions.CharacterSwap -= SetCharacter;
    }

    void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();
        charManager = FindObjectOfType<CharacterManager>();
        character = charManager.Character;
    }

    void Start()
    {
        col = GetComponent<BoxCollider>();
        if (col == null)
        {
            col = GetComponent<CapsuleCollider>();
        }
        if (col == null)
        {
            Debug.LogError("No collider component found on " + gameObject.name);
        }
        character = charManager.Character;
    }

    void SetCharacter(GameObject newCharacter)
    {
        Character = newCharacter;
    }

    void Update()
    {

        col.enabled = (character.transform.position - transform.position).sqrMagnitude < Distance * Distance;

        if (col.enabled)
        {
            Clickable.CloseEnoughToClick = true;
        }
        else
        {
            Clickable.CloseEnoughToClick = false;
        }
    }

}

