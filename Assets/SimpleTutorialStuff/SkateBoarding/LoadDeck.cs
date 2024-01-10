using UnityEngine;
using System.IO;
using UnityEngine.UI;
using SFB;

public class LoadDeck : MonoBehaviour
{
    public RawImage drawingCanvas;
    private Texture2D originalTexture;
    public PixelMaterialEditor pixelMaterialEditor;

    public int desiredWidth = 128;
    public int desiredHeight = 48;

    public void LoadDrawingFromPNG()
    {
        // Open a file dialog for selecting an image file
        var extensions = new[]
        {
            new SFB.ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
            new SFB.ExtensionFilter("All Files", "*"),
        };

        string[] filePaths = SFB.StandaloneFileBrowser.OpenFilePanel("Load Drawing from PNG", "", extensions, false);

        // Check if a file was selected
        if (filePaths != null && filePaths.Length > 0)
        {
            string filePath = filePaths[0];

            try
            {
                // Read the selected image file as bytes
                byte[] imageData = File.ReadAllBytes(filePath);

                // Create a new Texture2D to load the image
                Texture2D loadedTexture = new Texture2D(2, 2);
                if (loadedTexture.LoadImage(imageData))
                {
                    // Set the desired resolution (e.g., 128x48) when calling LoadImageTexture method.
                    pixelMaterialEditor.LoadImageTexture(loadedTexture, desiredWidth, desiredHeight);

                    // Log a message to indicate successful loading
                    Debug.Log("Drawing loaded from: " + filePath);
                }
                else
                {
                    Debug.LogError("Failed to load image from: " + filePath);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading image: " + ex.Message);
            }
        }
    }
}
