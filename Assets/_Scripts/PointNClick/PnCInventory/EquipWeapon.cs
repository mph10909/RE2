using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class EquipWeapon : MonoBehaviour
    {
        public Item item;
        public Transform inventoryButtons;

        void OnDisable()
        {
            item = null;
        }

        public void OnClick_EquipThisWeapon()
        {
            if (item.IsWeapon())
            {
                SoundManagement.Instance.MenuSound(MenuSounds.Accept);
                Actions.SetWeapon?.Invoke(item);
            }
            else if (item.IsUsable())
            {
                Actions.UseItem?.Invoke(item);
            }
            else
            {
                SoundManagement.Instance.MenuSound(MenuSounds.Decline);
                return;
            }

            foreach (Transform child in inventoryButtons)
            {
                if (child.name == "ItemTemplate") continue;
                else
                {
                    IUnselectable unSelect = child.gameObject.GetComponent<IUnselectable>();
                    unSelect.UnSelect();
                }
            }
        }

    }
}
