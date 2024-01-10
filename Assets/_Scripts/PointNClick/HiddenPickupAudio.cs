using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace ResidentEvilClone
{

    public class HiddenPickupAudio : HiddenObjectPickup
    {
        [SerializeField] AudioClip backtoAudio;

        public UnityEvent ItemTaken;
        public UnityEvent ItemLoaded;

        public override void PickUp()
        {
            SoundManagement.Instance.SwapTrack(0.5f, backtoAudio);
            base.PickUp();
            ItemTaken?.Invoke();

        }

        public override void SetFromSaveData(string savedData)
        {
            ObjectPickedup = Convert.ToBoolean(savedData);

            if (ObjectPickedup)
            {
                ItemLoaded?.Invoke();
            }
        }
    }

}

