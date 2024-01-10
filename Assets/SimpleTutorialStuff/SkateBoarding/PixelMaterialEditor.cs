using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PixelMaterialEditor : MonoBehaviour
{
    public RawImage drawingCanvas;
    public Material pixelMaterial;
    public MeshRenderer meshRenderer;

    public Texture2D startingTexture;

    private Texture2D drawingTexture;
    public Texture2D loadedImageTexture;

    private bool isDrawing = false;
    private Vector2 lastDrawPosition;

    public int pixelBrushSize = 1;
    public Color drawingColor = Color.black;
    public bool isFilling = false;

    public DrawingMode currentDrawingMode = DrawingMode.Brush;
    private Vector2 lineStartPos;

    private Texture2D tempDrawingTexture;
    private Texture2D originalDrawingTexture;
    private Stack<Texture2D> drawingHistory = new Stack<Texture2D>();

    private void Start()
    {
        // Create the drawing texture with the same size as the Raw Image.
        drawingTexture = new Texture2D((int)drawingCanvas.rectTransform.rect.width, (int)drawingCanvas.rectTransform.rect.height, TextureFormat.RGBA32, false);
        drawingTexture.filterMode = FilterMode.Point;

        Color[] blankDeckPixels = startingTexture.GetPixels();

        // Load the pixel data into the drawing texture.
        drawingTexture.SetPixels(blankDeckPixels);
        drawingTexture.Apply();

        // Create the temporary texture with the same size as the drawing texture.
        tempDrawingTexture = new Texture2D(drawingTexture.width, drawingTexture.height, TextureFormat.RGBA32, false);
        tempDrawingTexture.filterMode = FilterMode.Point;
        // Apply the drawing texture to the Raw Image.
        drawingCanvas.texture = drawingTexture;
    }

    public void SetDrawingColor(Color color)
    {
        drawingColor = color;
    }

    public void SetDrawingModeToBrush()
    {
        currentDrawingMode = DrawingMode.Brush;
    }

    public void SetDrawingModeToLine()
    {
        currentDrawingMode = DrawingMode.Line;

        // Reset the line starting position to avoid unexpected behavior.
        lineStartPos = Vector2.zero;
    }

    public void SetDrawingModeToRectangle()
    {
        currentDrawingMode = DrawingMode.Rectangle;

        // Reset the rectangle starting position to avoid unexpected behavior.
        lineStartPos = Vector2.zero;
    }

    public void SetDrawingModeToCircle()
    {
        currentDrawingMode = DrawingMode.Circle;

        // Reset the rectangle starting position to avoid unexpected behavior.
        lineStartPos = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = IsMouseOverCanvas();
            lastDrawPosition = GetLocalMousePosition();

            // Set the starting position for the line.
            lineStartPos = lastDrawPosition;

            // If drawing has started, make a copy of the current drawing texture for undo.
            if (isDrawing)
            {
                originalDrawingTexture = new Texture2D(drawingTexture.width, drawingTexture.height, TextureFormat.RGBA32, false);
                Graphics.CopyTexture(drawingTexture, originalDrawingTexture);
            }

            // Copy the current drawing texture to the temporary texture.
            Graphics.CopyTexture(drawingTexture, tempDrawingTexture);
        }

        // Check if the left mouse button is released to stop drawing.
        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        // Draw on the texture if the left mouse button is held down.
        if (isDrawing)
        {
            if (isFilling && currentDrawingMode == DrawingMode.Brush)
            {
                FillTextureWithColor();
            }
            else
            {
                Vector2 drawPosition = GetLocalMousePosition();

                // Check the drawing mode and either draw the line or use the brush.
                if (currentDrawingMode == DrawingMode.Brush)
                {
                    DrawOnTexture(drawPosition, lastDrawPosition);
                }
                else if (currentDrawingMode == DrawingMode.Line)
                {
                    // Copy the temporary texture back to the drawing texture.
                    Graphics.CopyTexture(tempDrawingTexture, drawingTexture);

                    // Draw the line directly on the drawing texture (without applying the brush).
                    DrawLineOnTexture((int)lineStartPos.x, (int)lineStartPos.y, (int)drawPosition.x, (int)drawPosition.y, drawingColor, pixelBrushSize);
                }
                else if (currentDrawingMode == DrawingMode.Rectangle)
                {
                    // Copy the temporary texture back to the drawing texture.
                    Graphics.CopyTexture(tempDrawingTexture, drawingTexture);

                    // Draw the line directly on the drawing texture (without applying the brush).
                    DrawRectangleOnTexture((int)lineStartPos.x, (int)lineStartPos.y, (int)drawPosition.x, (int)drawPosition.y, drawingColor, pixelBrushSize);
                }
                else if (currentDrawingMode == DrawingMode.Circle)
                {
                    // Copy the temporary texture back to the drawing texture.
                    Graphics.CopyTexture(tempDrawingTexture, drawingTexture);

                    // Draw the circle directly on the texture (without applying the brush).
                    DrawCircleOnTexture((int)lineStartPos.x, (int)lineStartPos.y, (int)drawPosition.x, (int)drawPosition.y, drawingColor, pixelBrushSize);
                }

                // Update the last draw position to the current position.
                lastDrawPosition = drawPosition;
            }
        }
    }

    private void DrawOnTexture(Vector2 currentPos, Vector2 lastPos)
    {
        // Draw brush or line based on the current drawing mode
        if (currentDrawingMode == DrawingMode.Brush)
        { 
            int x0 = Mathf.RoundToInt(lastPos.x);
            int y0 = Mathf.RoundToInt(lastPos.y);
            int x1 = Mathf.RoundToInt(currentPos.x);
            int y1 = Mathf.RoundToInt(currentPos.y);

            DrawLineOnTexture(x0, y0, x1, y1, drawingColor, pixelBrushSize);
        }
        else if (currentDrawingMode == DrawingMode.Line)
        {
            DrawLineOnTexture((int)lastPos.x, (int)lastPos.y, (int)currentPos.x, (int)currentPos.y, drawingColor, pixelBrushSize);
        }
    }



    private void DrawLineOnTexture(int x0, int y0, int x1, int y1, Color color, int brushSize)
    {
        Vector2 delta = new Vector2(x1 - x0, y1 - y0);
        int steps = Mathf.RoundToInt(delta.magnitude) * brushSize;
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            int x = Mathf.RoundToInt(Mathf.Lerp(x0, x1, t));
            int y = Mathf.RoundToInt(Mathf.Lerp(y0, y1, t));
            DrawBrushOnTexture(x, y, color, brushSize);
        }

        // Apply the changes to the texture.
        drawingTexture.Apply();
    }

    private void DrawCircleOnTexture(int x0, int y0, int x1, int y1, Color color, int brushSize)
    {
        if (isFilling)
        {
            // If filling is enabled, fill the oval using the FillTextureWithColor method.
            Vector2 clickPosition = GetLocalMousePosition();
            int startX = (int)lineStartPos.x;
            int startY = (int)lineStartPos.y;
            int endX = (int)clickPosition.x;
            int endY = (int)clickPosition.y;

            // Calculate the bounds of the oval to ensure we iterate over the correct pixels.
            int minX = Mathf.Min(startX, endX);
            int minY = Mathf.Min(startY, endY);
            int maxX = Mathf.Max(startX, endX);
            int maxY = Mathf.Max(startY, endY);

            // Iterate over the pixels and fill the oval.
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float normalizedX = Mathf.InverseLerp(minX, maxX, x);
                    float normalizedY = Mathf.InverseLerp(minY, maxY, y);

                    // Convert the normalized position to local position of the Raw Image.
                    float localX = Mathf.Lerp(startX, endX, normalizedX);
                    float localY = Mathf.Lerp(startY, endY, normalizedY);

                    // Check if the current pixel is within the oval shape using the oval equation.
                    float a = (maxX - minX) * 0.5f;
                    float b = (maxY - minY) * 0.5f;
                    float xC = (maxX + minX) * 0.5f;
                    float yC = (maxY + minY) * 0.5f;
                    float xVal = localX - xC;
                    float yVal = localY - yC;
                    float result = (xVal * xVal) / (a * a) + (yVal * yVal) / (b * b);

                    if (result <= 1f)
                    {
                        // Set the pixel color to the drawing color.
                        SetPixelWithBrush(x, y, drawingColor);
                    }
                }
            }

            // Apply the changes to the texture.
            drawingTexture.Apply();
        }
        else
        {
        // Ensure x0 < x1 and y0 < y1 to get correct bounds for the oval.
        int minX = Mathf.Min(x0, x1);
        int minY = Mathf.Min(y0, y1);
        int maxX = Mathf.Max(x0, x1);
        int maxY = Mathf.Max(y0, y1);

        int width = maxX - minX;
        int height = maxY - minY;
        int radiusX = width / 2;
        int radiusY = height / 2;
        int centerX = minX + radiusX;
        int centerY = minY + radiusY;

        int numSegments = 360;
        float angleIncrement = 360f / numSegments;

        for (int i = 0; i < numSegments; i++)
        {
            float angleRad = Mathf.Deg2Rad * (i * angleIncrement);
            int x = Mathf.RoundToInt(centerX + radiusX * Mathf.Cos(angleRad));
            int y = Mathf.RoundToInt(centerY + radiusY * Mathf.Sin(angleRad));
            DrawBrushOnTexture(x, y, color, brushSize);
        }

        // Apply the changes to the texture.
        drawingTexture.Apply();
        }

    }

    private void DrawRectangleOnTexture(int x0, int y0, int x1, int y1, Color color, int brushSize)
    {
        // Ensure x0 < x1 and y0 < y1 to get correct bounds for the rectangle.
        int minX = Mathf.Min(x0, x1);
        int minY = Mathf.Min(y0, y1);
        int maxX = Mathf.Max(x0, x1);
        int maxY = Mathf.Max(y0, y1);

        if (!isFilling)
        {
        for (int x = minX; x <= maxX; x++)
        {
            DrawBrushOnTexture(x, minY, color, brushSize);
            DrawBrushOnTexture(x, maxY, color, brushSize);
        }

        for (int y = minY; y <= maxY; y++)
        {
            DrawBrushOnTexture(minX, y, color, brushSize);
            DrawBrushOnTexture(maxX, y, color, brushSize);
        }
        }
        else
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    SetPixelWithBrush(x, y, color);
                }
            }
        }


        // Apply the changes to the texture.
        drawingTexture.Apply();
    }

    private void DrawBrushOnTexture(int x, int y, Color color, int brushSize)
    {
        int startX = Mathf.RoundToInt(x - (brushSize - 1) / 2f);
        int startY = Mathf.RoundToInt(y - (brushSize - 1) / 2f);

        
        // Draw a square brush using Bresenham's line algorithm.
        for (int i = 0; i < brushSize; i++)
        {
            for (int j = 0; j < brushSize; j++)
            {
                int drawX = startX + i;
                int drawY = startY + j;

                if (drawX >= 0 && drawX < drawingTexture.width && drawY >= 0 && drawY < drawingTexture.height)
                {
                    // Get the existing color on the texture.
                    Color existingColor = drawingTexture.GetPixel(drawX, drawY);

                    // Blend the drawn color with the existing color based on the drawn color's alpha value.
                    Color blendedColor = Color.Lerp(existingColor, color, color.a);

                    drawingTexture.SetPixel(drawX, drawY, blendedColor);
                }
            }
        }

        // Apply the changes to the texture.
        drawingTexture.Apply();
    }

    private void SetPixelWithBrush(int x, int y, Color color)
    {
        // Set the pixels on the drawing texture with the selected color.
        if (x >= 0 && x < drawingTexture.width && y >= 0 && y < drawingTexture.height)
        {
            // Get the existing color on the texture.
            Color existingColor = drawingTexture.GetPixel(x, y);

            // Blend the drawn color with the existing color based on the drawn color's alpha value.
            Color blendedColor = Color.Lerp(existingColor, color, color.a);

            drawingTexture.SetPixel(x, y, blendedColor);
        }
    }

    private bool IsMouseOverCanvas()
    {
        // Check if the mouse is within the bounds of the RawImage
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, Input.mousePosition, null, out localPoint))
        {
            return drawingCanvas.rectTransform.rect.Contains(localPoint);
        }

        return false;
    }

    public void ToggleFillingMode()
    {
        isFilling = !isFilling; // Toggle between drawing and filling modes
    }

    public void FillTextureWithColor()
    {
        Vector2 clickPosition = GetLocalMousePosition();
        int startX = Mathf.RoundToInt(clickPosition.x);
        int startY = Mathf.RoundToInt(clickPosition.y);

        Color targetColor = drawingTexture.GetPixel(startX, startY);

        // Avoid infinite loop for self-color filling
        if (drawingColor == targetColor)
        {
            return;
        }

        Queue<Vector2Int> pixelsToFill = new Queue<Vector2Int>();
        pixelsToFill.Enqueue(new Vector2Int(startX, startY));

        while (pixelsToFill.Count > 0)
        {
            Vector2Int pixel = pixelsToFill.Dequeue();
            int x = pixel.x;
            int y = pixel.y;

            if (x >= 0 && x < drawingTexture.width && y >= 0 && y < drawingTexture.height)
            {
                if (drawingTexture.GetPixel(x, y) == targetColor)
                {
                    drawingTexture.SetPixel(x, y, drawingColor);
                    pixelsToFill.Enqueue(new Vector2Int(x - 1, y));
                    pixelsToFill.Enqueue(new Vector2Int(x + 1, y));
                    pixelsToFill.Enqueue(new Vector2Int(x, y - 1));
                    pixelsToFill.Enqueue(new Vector2Int(x, y + 1));
                }
            }
        }

        // Apply the changes to the texture.
        drawingTexture.Apply();
    }

    public void LoadImageTexture(Texture2D texture, int width, int height)
    {
        // Load the provided texture as the base image to draw on
        loadedImageTexture = texture;

        // Create a new drawing texture with the specified dimensions.
        drawingTexture = new Texture2D(loadedImageTexture.width, loadedImageTexture.height, TextureFormat.RGBA32, false);
        drawingTexture.filterMode = FilterMode.Point;
        drawingTexture.SetPixels(loadedImageTexture.GetPixels());
        drawingTexture.Apply();

        // Resize the drawing texture to the specified resolution.
        ResizeTexture(drawingTexture, width, height);

        // Assign the drawing texture to the Raw Image
        drawingCanvas.texture = drawingTexture;
    }

    public void ClearDrawingTexture()
    {
        loadedImageTexture = startingTexture;

        drawingTexture = new Texture2D(loadedImageTexture.width, loadedImageTexture.height, TextureFormat.RGBA32, false);
        drawingTexture.filterMode = FilterMode.Point;
        drawingTexture.SetPixels(loadedImageTexture.GetPixels());
        drawingTexture.Apply();

        // Assign the drawing texture to the Raw Image
        drawingCanvas.texture = drawingTexture;

        ApplyDrawingToMesh();
    }

    public void ApplyDrawingToMesh()
    {
        // Clone the drawing texture to create a new texture to apply to the mesh.
        Texture2D finalTexture = new Texture2D(drawingTexture.width, drawingTexture.height, TextureFormat.RGBA32, false);
        finalTexture.filterMode = FilterMode.Point;
        Graphics.CopyTexture(drawingTexture, finalTexture);

        // Set the updated texture to the material of the mesh.
        meshRenderer.material.SetTexture("_DrawingTex", finalTexture);
    }

    private void ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        // Create a temporary RenderTexture to hold the resized texture.
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
        rt.useMipMap = false;
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;

        // Copy the source texture to the temporary RenderTexture with the desired size.
        Graphics.Blit(sourceTexture, rt);

        // Create a new texture to store the resized result.
        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
        resizedTexture.filterMode = FilterMode.Point;

        // Read the pixels from the temporary RenderTexture into the new resized texture.
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();

        // Set the resized texture as the drawing texture.
        drawingTexture = resizedTexture;

        // Clean up the temporary RenderTexture.
        RenderTexture.active = null;
        rt.Release();
        Destroy(rt);
    }

    private Vector2 GetLocalMousePosition()
    {
        // Get the position of the mouse pointer in screen space.
        Vector2 mouseScreenPos = Input.mousePosition;

        // Convert the screen space position to canvas space using the canvas scaler.
        Vector2 canvasSpacePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.GetComponent<RectTransform>(), mouseScreenPos, null, out canvasSpacePos);

        // Convert the canvas space position to local position of the Raw Image.
        Vector2 localPos = canvasSpacePos - drawingCanvas.rectTransform.rect.min;

        // Normalize the position to the range [0, 1].
        float normalizedX = Mathf.InverseLerp(0, drawingCanvas.rectTransform.rect.width, localPos.x);
        float normalizedY = Mathf.InverseLerp(0, drawingCanvas.rectTransform.rect.height, localPos.y);

        // Convert normalized position to pixel position in the drawing texture.
        int pixelX = Mathf.RoundToInt(normalizedX * drawingTexture.width);
        int pixelY = Mathf.RoundToInt(normalizedY * drawingTexture.height);

        return new Vector2(pixelX, pixelY);
    }

    public void UndoLastDrawing()
    {
        // Check if there is an original texture to revert to.
        if (originalDrawingTexture != null)
        {
            // Restore the drawing texture to the original state before any drawing.
            Graphics.CopyTexture(originalDrawingTexture, drawingTexture);

            // Apply the changes to the texture.
            drawingTexture.Apply();
        }
    }

}
