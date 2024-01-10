using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class CheckItem : MonoBehaviour
    {
        [HideInInspector] public Item item;
        [SerializeField] GameObject checkPanel, items, namePanel;
        [SerializeField] Transform inspectorLocation;
        [SerializeField] Text descriptionText;
        [SerializeField] Button[] menuButtons;
        GameObject instaniateObject;

        const string BLANK = "";

        public void OnClick_Check()
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            instaniateObject = item.GetInspectorObject();
            descriptionText.text = item.GetDescriptionText();
            namePanel.SetActive(false);
            checkPanel.SetActive(true);
            items.SetActive(false);
            Instantiate(instaniateObject, inspectorLocation);
            foreach(Button button in menuButtons)
            {
                button.interactable = false;
            }
        }

        public void OnClick_CancelCheck()
        {
            if (checkPanel.activeSelf == false) return;
            descriptionText.text = BLANK;
            namePanel.SetActive(true);
            checkPanel.SetActive(false);
            items.SetActive(true);
            foreach (Button button in menuButtons)
            {
                button.interactable = true;
            }
            SoundManagement.Instance.MenuSound(MenuSounds.Cancel);
        }

    }
}
