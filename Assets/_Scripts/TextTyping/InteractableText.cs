using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class InteractableText : MonoBehaviour
    {

        [SerializeField][TextArea(3,5)] string text;
        [SerializeField] private bool typerwriter;

        public void Interact()
        {
            UIText.Instance.StartDisplayingText(text, typerwriter);
        }
    }
}
