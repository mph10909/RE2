using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResidentEvilClone
{

    public class MultiLineTextObject : DisplayText, IInteractable
    {
        [TextArea(3,5)] public string[] text;
        public TextData textData;
        int startingText = 0;
        bool isFinished;
        bool isActive;

        bool usingTextData;

        public int TextProgress { get { return startingText; } }
        public bool IsFinished {  get { return isFinished; } }
        public bool IsActive { get { return isActive; } }

        public virtual void Interact()
        {
            if (textData != null) usingTextData = true;
            if (startingText != 0) return;
            isActive = true;
            isFinished = false;
            Time.timeScale = 0;
            if(usingTextData) UnclearableTextDisplay(textData.body[startingText]);
            else UnclearableTextDisplay(text[startingText]);
            
        }

        void Update()
        {
            if (!isActive) return;
            if (Input.GetMouseButtonDown(0))
            {
                LastLine();
                NextTextLine();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Finished();
            }
        }

        public void NextTextLine()
        {
            if (isFinished) return;
            startingText++;
            if (usingTextData) UnclearableTextDisplay(textData.body[startingText]);
            else UnclearableTextDisplay(text[startingText]);
        }

        public void LastLine()
        {
            if (usingTextData)
            {
                if (startingText == textData.body.Length - 1)
                {
                    Finished();
                    return;
                }
            }
            else
            {
                if (startingText == text.Length-1)
                {
                    Finished();
                    return;
                }
            }

        }

        public void Finished()
        {
            isFinished = true;
            isActive = false;
            Time.timeScale = 1;
            TextClear();
            startingText = 0;
            return;
        }
    }
}
