using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    public Color color; // The color assigned to this swatch.

    private Image swatchImage;
    public PixelMaterialEditor pixelMaterialEditor;
    public SelectedColor selectedColor;
    public Button button;

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
        button = GetComponent<Button>();
        swatchImage = GetComponent<Image>();
        color = swatchImage.color;
        pixelMaterialEditor = FindObjectOfType<PixelMaterialEditor>();
        selectedColor = FindObjectOfType<SelectedColor>();
    }

    public void OnSwatchClicked()
    {
        // When the swatch is clicked, set the color as the current drawing color.
        pixelMaterialEditor.SetDrawingColor(color);
        selectedColor.SetSelectedColor(color);
    }
}