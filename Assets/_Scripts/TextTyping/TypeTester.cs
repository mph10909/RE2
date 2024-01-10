using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class TypeTester : MonoBehaviour
    {
        public Text_Data testTextData;
        public bool typewriter;// Assign this from the inspector

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartTextDisplay();
            }
        }

        private void StartTextDisplay()
        {
            // Check if the UIText instance is available and there is text data to display
            if (UIText.Instance != null && testTextData != null)
            {

                // Start displaying text
                UIText.Instance.StartDisplayingText(testTextData, typewriter);
            }
        }

    }
}
