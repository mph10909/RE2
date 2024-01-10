using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class HiddenTextObject : MonoBehaviour
    {

        [SerializeField] [TextArea(3,5)] string text;
        [SerializeField] [TextArea(3,5)] string hiddenText;
        [SerializeField] int textCounter;
        [SerializeField] int readAmount;

        public void Interact()
        {
            if(readAmount != textCounter)
            {
                UIText.Instance.StartDisplayingText(text, false);
                readAmount++;
            }
            else
            {
                UIText.Instance.StartDisplayingText(hiddenText, false);
                readAmount = 0;
            }



        }

    }
}
