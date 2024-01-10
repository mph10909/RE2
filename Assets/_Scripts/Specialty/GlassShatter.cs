using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class GlassShatter : MonoBehaviour, IDestroyable, IComponentSavable
    {
        [SerializeField] ParticleSystem glass;
        [SerializeField] Texture shatteredTexture;
        [SerializeField] AudioClip shatteredAudio;
        [SerializeField]bool shattered;
        Renderer startTexture;

        void Start()
        {
            startTexture = GetComponent<Renderer>();
        }

        public void Destroy()
        {
            if (!shattered)
            {
                SoundManagement.Instance.PlaySound(shatteredAudio);
                Shatter();
            }
        }

        public void EventDestroy()
        {
            Shatter();
        }

        private void Shatter()
        {
            if (glass != null) glass.Play();
            startTexture.material.SetTexture("_MainTex", shatteredTexture);
            shattered = true;
        }

        string ISavable<string>.GetSavableData()
        {
            return shattered.ToString();
        }

        void ISavable<string>.SetFromSaveData(string savedData)
        {
            shattered = Convert.ToBoolean(savedData);

            if(shattered) startTexture.material.SetTexture("_MainTex", shatteredTexture);
        }
    }
}
