using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class TutorialInspectionItems : MonoBehaviour, IInteractable
    {
        [SerializeField] string inspectionText;

        public void Interact()
        {
            TutorialActionMediator.TextMessage?.Invoke(inspectionText);
        }

    }
}
