using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class ButtonOnEnable : MonoBehaviour
    {
        Button thisButton;

        void OnEnable()
        {
            thisButton = GetComponent<Button>();
            thisButton.enabled = true;
        }
    }
}
