using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour
{
    private ButtonHighlightManager highlightManager;
    private Color originalColor;
    private Button thisButton;
    private Image thisImage;

    void OnValidate()
    {
        WakeUP();
    }

    void Awake()
    {
        WakeUP();
    }

    private void WakeUP()
    {
        highlightManager = FindObjectOfType<ButtonHighlightManager>();
        thisButton = GetComponent<Button>();
        thisImage = GetComponent<Image>();
        originalColor = thisImage.color;
    }

    public void OnButtonSelect()
    {
        highlightManager.OnButtonSelect(GetComponent<Button>());
    }

    public void RevertColor()
    {
        thisImage.color = originalColor;
    }
}
