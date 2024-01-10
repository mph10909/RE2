using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ClickableColliderDetector : ClickableDetector
    {
        private Collider col;

        public void Awake()
        {
            col = GetComponent<Collider>();
            MessageBuffer<PlayerDetectDisables>.Subscribe(OnTriggerExits);
        }

        public void OnEnable()
        {
            col.enabled = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            col.enabled = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            Clickable clickable = other.GetComponent<Clickable>();
            if (clickable != null && clickable.isActiveAndEnabled)
            {
                AddClickable(clickable);
            }
        }

        public void OnTriggerExits(PlayerDetectDisables msg)
        {
            RemoveClickable(msg.ThisGameObject);
        }

        public void OnTriggerExit(Collider other)
        {
            RemoveClickable(other.gameObject);
        }


    }
}
