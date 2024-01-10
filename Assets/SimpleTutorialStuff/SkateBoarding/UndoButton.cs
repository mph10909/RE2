using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    public PixelMaterialEditor pixelMaterialEditor;

    private void Start()
    {
        // Find the PixelMaterialEditor script in the scene.
        pixelMaterialEditor = FindObjectOfType<PixelMaterialEditor>();
    }

    public void OnClickUndo()
    {
        pixelMaterialEditor.UndoLastDrawing();
    }
}

