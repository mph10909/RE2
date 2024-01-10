using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;


    public static class Actions
    {
        public static Action <bool, GameObject> OnPlayerKilled;
        public static Action OnEnemyLimbRemoved;
        public static Action FiredWeapon;
        public static Action <bool> DisableCharacterSwap;
        public static Action <GameObject> CharacterSwap;
        public static Action <GameObject> SetCamera;
        public static Action <bool> ClickedObject;
        public static Action CameraSwap;
        public static Action MusicSwap;
        public static Action SaveMenuOn;
        public static Action <string> TextSet;
        public static Action SetText;
        public static Action TextClear;
        public static Action InventoryItemChange;
        public static Action <Item> SetWeapon;
        public static Action <Item> WeaponToStorage;
        public static Action <Item> UseItem; 
        public static Action CombineItem;
        public static Action<Item,Item> CombineItems;
        public static Action RefreshInventory;
        public static Action EnemyForget;
        public static Action PathIsFinished;
        public static Action <string[]> TextBodySet;
        public static Action<Vector3> DestinationLocation;
        public static Action<bool> RotationComplete;
        public static Action<float> PauseCharacter;
        public static Action FadeTrigger;
        public static Action PauseTime;
        public static Action ResumeTime;
        public static Action AmmoEmpty;

        public static Action EnemyKilled;
}

