using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayerHealed : MonoBehaviour, IControllable
    {
        [SerializeField] PlayerAttacked health;
        [SerializeField] PlayerInventory CurrentPlayer;
        [SerializeField] GameObject increaseableGameObject;

        void OnEnable()
        {
            Actions.UseItem += HealPlayer;
        }
        void OnDisable()
        {
            Actions.UseItem -= HealPlayer;
        }

        void HealPlayer(Item item)
        {
            switch (item.itemType)
            {
                case Item.ItemType.First_Aid_Spray:
                    health.Heal(100);
                    health.SetHealthStats();
                    IncreaseItemUse();
                    ClearInventory(item);
                    break;
                case Item.ItemType.Green_Herb:
                    health.Heal(25);
                    health.SetHealthStats();
                    ClearInventory(item);
                    break;
                case Item.ItemType.Mixed_Herb_GG:
                    health.Heal(50);
                    health.SetHealthStats();
                    ClearInventory(item);
                    break;
                case Item.ItemType.Mixed_Herb_GR:
                    health.Heal(100);
                    health.SetHealthStats();
                    ClearInventory(item);
                    break;
                case Item.ItemType.Mixed_Herb_GGG:
                    health.Heal(100);
                    health.SetHealthStats();
                    ClearInventory(item);
                    break;
            }
        }

        private void IncreaseItemUse()
        {
            var increasable = increaseableGameObject.GetComponent<IIncreasable>();
            if (increasable == null) return;
            increasable.Increase();
            print(CurrentPlayer.name);
            return;
        }

        private void ClearInventory(Item item)
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            CurrentPlayer.inventory.RemoveItem(item);
            Actions.RefreshInventory?.Invoke();
        }

        public void EnableControl(bool enable)
        {
            this.enabled = enable;
        }
    }
}
