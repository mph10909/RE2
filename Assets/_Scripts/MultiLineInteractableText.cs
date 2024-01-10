using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{

    public class MultiLineInteractableText : MonoBehaviour
    {
        [TextArea(3,5)] public string[] text;
        [SerializeField] private bool typerwriter;

        public void Interact()
        {
            UIText.Instance.StartDisplayingText(text, typerwriter);
        }
    }
}
