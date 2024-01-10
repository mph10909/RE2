using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class MouseOver : MonoBehaviour
    {
        public void OnMouseOver()
        {
            if (gameObject.tag == "UI") print("UIBLOCK");
        }
    }
}
