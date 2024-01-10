using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public static Text TextObject;
    void Start()
    {
        TextObject = GetComponentInChildren<Text>();
    }

    public static void TEXT_DISABLER()
    {
        TextObject.text = "";
    }
}
