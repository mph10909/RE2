using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This class acts as a mediator to trigger events or actions in the game. 
public class TutorialActionMediator
{
    // Event to check if the player has a specific item in their inventory
    public static Action<bool> ContainsItem;

    // Event to check if a specific item is in the player's inventory
    public static Action<TutorialItem.TutItem> CheckForItem;

    // Event to trigger a change in location of an object
    public static Action<Transform> LocationChange;

    // Event to display text in the game
    public static Action<string> TextMessage;

    // Event to add an item to the player's inventory
    public static Action<TutorialItem> ItemToInventory;
}
