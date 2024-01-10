using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class DrawingCanvas : MonoBehaviour
{
    public PixelMaterialEditor pixelMaterialEditor;

    public RawImage rawImage;


    public Texture2D defaultCursorTexture;

    public Texture2D cursorBrush;
    public Texture2D cursorBrushFill;
    public Texture2D cursorShape;
    private RectTransform rawImageRect;

    public Vector2 fillHotspot = new Vector2(0, 0);
    public Vector2 ShapeHotspot = new Vector2(0, 0);

    void Start()
    {
        rawImageRect = rawImage.rectTransform;
    }

    void Update()
    {
        // Get the mouse position in screen space.
        Vector2 mouseScreenPos = Input.mousePosition;

        // Convert the mouse position to local position in the RawImage.
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageRect, mouseScreenPos, null, out localPoint))
        {
            // Check if the local position is within the bounds of the RawImage.
            bool isMouseOverRawImage = localPoint.x >= rawImageRect.rect.xMin && localPoint.x <= rawImageRect.rect.xMax &&
                                       localPoint.y >= rawImageRect.rect.yMin && localPoint.y <= rawImageRect.rect.yMax;

            // Set the cursor according to whether the mouse is over the RawImage.
            //Cursor.SetCursor(isMouseOverRawImage ? cursorTextureWhenOver : defaultCursorTexture, Vector2.zero, CursorMode.Auto);
            if (isMouseOverRawImage)
            {
                switch (pixelMaterialEditor.currentDrawingMode)
                {
                    case DrawingMode.Brush:
                        if(pixelMaterialEditor.isFilling)Cursor.SetCursor(cursorBrushFill, fillHotspot, CursorMode.Auto);
                        else Cursor.SetCursor(cursorBrush, Vector2.zero, CursorMode.Auto);
                        break;
                    case DrawingMode.Circle:
                        Cursor.SetCursor(cursorShape, ShapeHotspot, CursorMode.Auto);
                        break;
                    case DrawingMode.Rectangle:
                        Cursor.SetCursor(cursorShape, ShapeHotspot, CursorMode.Auto);
                        break;
                    case DrawingMode.Line:
                        Cursor.SetCursor(cursorShape, ShapeHotspot, CursorMode.Auto);
                        break;
                }
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}