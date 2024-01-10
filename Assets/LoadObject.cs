using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class LoadObject : MonoBehaviour, IILoadable
    {
        [SerializeField] bool activated = false;

        public void Load()
        {
            if (activated) this.gameObject.SetActive(false);
            if (!activated) this.gameObject.SetActive(true);
        }

        void Start()
        {
            if (activated) this.gameObject.SetActive(false);
            if(!activated) this.gameObject.SetActive(true);
        }

        void OnEnable()
        {
            activated = false;
        }

        void OnDisable()
        {
            activated = true;
        }
    }
}
