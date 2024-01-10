using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [CreateAssetMenu(fileName = "TextData", menuName = "Text Management/Text Data")]
    public class Text_Data : ScriptableObject
    {
        [TextArea]
        public string[] lines;
        public Color textColor = Color.white;
        public float typingSpeed = 0.05f;
        public AudioClip typingSound;
    }
}
