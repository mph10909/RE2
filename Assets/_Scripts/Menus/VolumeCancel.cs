using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ResidentEvilClone
{
    public class VolumeCancel : MonoBehaviour, IPointerUpHandler
    {
        public VolumeControl volumeControl;

        public void OnPointerUp(PointerEventData eventData)
        {
            print("Up");
            volumeControl.IsHeldDown = false;
            StopAllCoroutines();
        }
    }
}
