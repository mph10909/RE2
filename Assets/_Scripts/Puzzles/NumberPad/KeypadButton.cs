using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [Header("Configuration")]
    public int buttonValue;
    public bool isBackButton = false;
    public bool isEnterKey = false;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material highlightedMaterial;

    [Header("Controller & Renderer")]
    public KeypadController keypadController;
    private MeshRenderer meshRenderer;

    [Header("Neighboring Buttons")]
    public KeypadButton up;
    public KeypadButton down;
    public KeypadButton left;
    public KeypadButton right;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Returns the string representation of the button value.
    /// </summary>
    public string GetButtonValue()
    {
        return buttonValue.ToString();
    }

    /// <summary>
    /// Highlights the button using the assigned highlighted material.
    /// </summary>
    public void HighlightButton()
    {
        if (meshRenderer)
            meshRenderer.material = highlightedMaterial;
    }

    /// <summary>
    /// Resets the button's material to its default state.
    /// </summary>
    public void UnhighlightButton()
    {
        if (meshRenderer)
            meshRenderer.material = defaultMaterial;
    }

    /// <summary>
    /// Confirms the button's action. 
    /// This is a demonstration method and can be expanded for complex actions.
    /// </summary>
    public void ConfirmButton()
    {
        Debug.Log($"Button {buttonValue} confirmed!");
    }
}
