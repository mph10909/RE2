using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialS
{

    enum Item
    {
        GreenHerb,
        RedHerb,
        LargeGreenHerb,
        MixedGreenRedHerb
    }

    struct Recipe
    {
        public Item Item1;
        public Item Item2;
        public Item Result;

        public Recipe(Item item1, Item item2, Item result)
        {
            Item1 = item1;
            Item2 = item2;
            Result = result;
        }
    }

    class ButtonData
    {
        public Item Item;
        public Button Button;
        public bool Clicked = false;

        public ButtonData(Item item, Button button)
        {
            Item = item;
            Button = button;
        }
    }

    class ItemCombination : MonoBehaviour
    {
        public Button buttonPrefab;
        public Transform buttonContainer;
        public Recipe[] recipes;

        private List<ButtonData> buttons = new List<ButtonData>();
        private Item? selectedItem = null;

        void Start()
        {
            // Instantiate buttons with random items
            for (int i = 0; i < 6; i++)
            {
                Item item = (Item)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Item)).Length);
                Button button = Instantiate(buttonPrefab, buttonContainer);
                button.onClick.AddListener(() => OnButtonClick(button));
                button.GetComponentInChildren<Text>().text = item.ToString();
                buttons.Add(new ButtonData(item, button));

                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -40);
            }
        }

        void OnButtonClick(Button clickedButton)
        {
            // Get the ButtonData for the clicked button
            ButtonData clickedButtonData = buttons.Find(data => data.Button == clickedButton);

            if (selectedItem == null)
            {
                // First button clicked
                selectedItem = clickedButtonData.Item;
                clickedButtonData.Button.interactable = false;
                clickedButtonData.Clicked = true;

            }
            else
            {
                // Second button clicked
                foreach (ButtonData buttonData in buttons)
                {
                    if (buttonData != clickedButtonData && buttonData.Clicked)
                    {
                        // Find a recipe that matches the selected item and the clicked item
                        Recipe recipe = Array.Find(recipes, r =>
                            (r.Item1 == selectedItem && r.Item2 == clickedButtonData.Item) ||
                            (r.Item1 == clickedButtonData.Item && r.Item2 == selectedItem));

                        if (recipe.Result != 0)
                        {
                            // Remove the selected and clicked buttons
                            Destroy(clickedButtonData.Button.gameObject);
                            Destroy(buttonData.Button.gameObject);

                            // Instantiate a new button with the result item
                            Button newButton = Instantiate(buttonPrefab, buttonContainer);
                            newButton.onClick.AddListener(() => OnButtonClick(newButton));
                            newButton.GetComponentInChildren<Text>().text = recipe.Result.ToString();
                            buttons.Add(new ButtonData(recipe.Result, newButton));
                        }

                        selectedItem = null;
                        break;
                    }
                }
            }
        }
    }
}

