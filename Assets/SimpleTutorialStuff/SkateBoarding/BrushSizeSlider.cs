using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSlider : MonoBehaviour
{
    // Reference to the Slider component
    public Slider brushSizeSlider;
    public Text sizeText;

    // Reference to the PixelMaterialEditor script (assuming it's attached to the same GameObject)
    public PixelMaterialEditor pixelMaterialEditor;

    private void Start()
    {
        // Subscribe to the OnValueChanged event of the Slider component
        brushSizeSlider.onValueChanged.AddListener(OnBrushSizeValueChanged);
    }

    private void OnBrushSizeValueChanged(float value)
    {
        // Update the brush size in the PixelMaterialEditor script
        int brushSize = Mathf.RoundToInt(value);
        sizeText.text = brushSize.ToString();
        pixelMaterialEditor.pixelBrushSize = brushSize;

    }
}
