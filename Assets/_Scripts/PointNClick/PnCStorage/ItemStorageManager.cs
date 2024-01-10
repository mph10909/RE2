using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResidentEvilClone
{
    public class ItemStorageManager : MonoBehaviour
    {
        public static Action RefreshInventory;
        public static Action<Item> RemoveItem;
        public static Action<Item> AddItem;
        public static Action<bool> MovingItemToInventory;
        public static Action<bool> Scrolling;
        public static Action<bool> ButtonEnable;
        public static Action SurroundOff;
        public static Item item;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            CursorControl.instance.Default();
            GameStateManager.Instance.SetState(GameState.Paused);
            
        }

        void OnDisable()
        {
            GameStateManager.Instance.SetState(GameState.GamePlay);
            
        }
    }
}
