using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB; // Import the SimpleFileBrowser namespace

public class SaveTextureAsPNG : MonoBehaviour
{
    public RawImage drawingCanvas;

    public void SaveDrawingAsPNG()
    {
        // Ensure the drawingCanvas has a texture assigned
        if (drawingCanvas.texture == null)
        {
            Debug.LogWarning("Drawing texture is null. Nothing to save.");
            return;
        }

        // Get the texture from the drawingCanvas
        Texture2D drawingTexture = drawingCanvas.texture as Texture2D;

        // Convert the texture to PNG data
        byte[] pngData = drawingTexture.EncodeToPNG();

        // Check if the texture data is valid
        if (pngData == null || pngData.Length == 0)
        {
            Debug.LogError("Failed to encode texture to PNG.");
            return;
        }

        // Define a default file name and extension
        string defaultFileName = "Drawing.png";
        var extensions = new[]
        {
            new SFB.ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
        };

        // Open a file save dialog box using SFB
        string filePath = SFB.StandaloneFileBrowser.SaveFilePanel("Save Drawing as PNG", "", defaultFileName, extensions);

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        try
        {
            // Write the PNG data to the selected file path
            File.WriteAllBytes(filePath, pngData);

            // Log a message to indicate successful saving
            Debug.Log("Drawing saved as PNG: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving PNG: " + ex.Message);
        }
    }
}
