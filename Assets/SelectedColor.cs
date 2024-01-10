using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedColor : MonoBehaviour
{
    public Color color; // The color assigned to this swatch.

    private Image swatchImage;

    private void OnValidate()
    {
        WakeUp();
    }

    private void Awake()
    {
        WakeUp();
    }

    private void WakeUp()
    {
        swatchImage = GetComponent<Image>();
        swatchImage.color = color;
    }

    public void SetSelectedColor(Color newColor)
    {
        swatchImage.color = newColor;
    }
}
