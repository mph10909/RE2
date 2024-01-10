using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickableCursor;
    public static CursorControl instance;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void Start()
    {
        Default();
    }

    public void Clickable()
    {
        Cursor.SetCursor(clickableCursor, Vector2.zero, CursorMode.Auto);
    }
    public void Default()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
