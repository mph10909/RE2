using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonHighlightManager : MonoBehaviour
{
    private Button selectedButton;
    [SerializeField] Button startButton;
    [SerializeField] Color highLightedColor;
    [SerializeField] List<Button> selectableButtons;

    void Start()
    {
        OnButtonSelect(startButton);
    }

    public void OnButtonSelect(Button button)
    {

        foreach(Button checkbutton in selectableButtons)
        {
            if(checkbutton == button)
            {
                selectedButton = button;
                selectedButton.image.color = highLightedColor;
            }
            else
            {
                checkbutton.GetComponent<ButtonHighlight>().RevertColor();
            }

        }

    }
}
