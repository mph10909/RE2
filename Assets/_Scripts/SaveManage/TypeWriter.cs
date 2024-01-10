using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;


namespace ResidentEvilClone
{
    public class TypeWriter : DisplayText, IInteractable
    {
        [SerializeField] Item.ItemType inkRibbon;
        [SerializeField] GameObject saveMenu, toSave;
        [SerializeField] AudioClip selectSound, declineSound, cancelSound;
        [SerializeField] [TextArea(3,5)]string noInk = "";
        [SerializeField] [TextArea(3,5)]string haveInk = "";
        [SerializeField] SaveManager saveManager;

        PlayerInventory characterInventory;
        GameObject character;
        //static bool makingDecision;

        void OnEnable()
        {
            saveManager = FindObjectOfType<SaveManager>();
            Actions.CharacterSwap += SetCharacter;
            //Actions.TextClear += TextClear;
        }

        void OnDisable()
        {
            Actions.CharacterSwap -= SetCharacter;
        }

        void SetCharacter(GameObject newCharacter)
        {
            character = newCharacter;
            characterInventory = newCharacter.GetComponent<PlayerInventory>();
        }

        public void Interact()
        {
            CheckForInkRibbon();
        }

        void CheckForInkRibbon()
        {
            foreach (Item item in characterInventory.inventory.GetItemList())
            {
                if (item.itemType == inkRibbon && item.amount > 0)
                {
                    Time.timeScale = 0;
                    TextDisplay(haveInk);
                    toSave.SetActive(true);
                    return;
                }
            }
            NoInk();
        }

        public void TextClear(InputAction.CallbackContext context)
        {
            //if (!makingDecision) return;
            //if(context.canceled)
            //{
            //    Actions.TextSet?.Invoke("");
            //    Time.timeScale = 1;
            //}
        }

        void NoInk()
        {
            //makingDecision = true;
            Time.timeScale = 0;
            //Actions.TextSet?.Invoke(noInk);
            TextDisplay(noInk);
        }

        public void OnClick_InkRibbonYes()
        {
            SoundManagement.Instance.PlaySound(selectSound);
            Actions.TextSet?.Invoke("");
            toSave.SetActive(false);
            StartCoroutine(Save(0.15f, 0));
        }

        public void OnClick_InkRibbonNo()
        {
            SoundManagement.Instance.PlaySound(cancelSound);
            Actions.TextSet?.Invoke("");
            GameStateManager.Instance.SetState(GameState.GamePlay);
            print("Back To Game");
            Time.timeScale = 1;
            toSave.SetActive(false);
        }

        public void OnClick_Save(int saveFile)
        {
            // Check if the character has any ink ribbons
            Item inkRibbon = characterInventory.inventory.GetItem(Item.ItemType.Ink_Ribbon);
            if (inkRibbon != null && inkRibbon.amount > 0)
            {
                // Play select sound and save the game
                SoundManagement.Instance.PlaySound(selectSound);
                characterInventory.inventory.RemoveItem(new Item { itemType = Item.ItemType.Ink_Ribbon, amount = 1 });
                saveManager.TyperWriterSave(saveFile);
                //SaveDataManager.Instance.SaveSlot();
                return;
            }

            // Play decline sound if the character has no ink ribbons
            SoundManagement.Instance.PlaySound(declineSound);
        }

        //public void OnClick_Save(int saveFile)
        //{
        //    foreach (Item item in characterInventory.inventory.GetItemList())
        //    {
        //        if (item.itemType == inkRibbon && item.amount > 0)
        //        {
        //            SoundManagement.Instance.PlaySound(selectSound);
        //            characterInventory.inventory.RemoveItem(
        //                new Item { itemType = Item.ItemType.Ink_Ribbon, amount = 1 });
        //            saveManager.TyperWriterSave(saveFile);
        //            return;
        //        }
        //    }
        //    SoundManagement.Instance.PlaySound(declineSound);
        //}

        public void OnClick_Cancel()
        {
            GameStateManager.Instance.SetState(GameState.GamePlay);
        }

        IEnumerator Save(float time, float timeScale)
        {
            Time.timeScale = timeScale;
            Fader.Instance.FadeOut(time, true);
            yield return new WaitForSecondsRealtime(time);
            saveMenu.SetActive(true);
        }
    }
}
