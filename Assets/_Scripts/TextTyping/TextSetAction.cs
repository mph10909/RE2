using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class TextSetAction : MonoBehaviour
    {
        [SerializeField] Text text;

        void Start()
        {
            text = GetComponent<Text>();
        }

        void OnEnable()
        {
            Actions.TextSet += TextSet;
        }
        void OnDisable()
        {
            Actions.TextSet -= TextSet;
        }

        void TextSet(string displayText)
        { 
            text.text = displayText;
        }
    }
}
