using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone

{
    public class ClickableChangedMessage : BaseMessage
    {
        public Clickable clickable;
    }

    public class ClickableDetector : MonoBehaviour
    {
        public List<Clickable> clickables = new List<Clickable>();
        private ClickableChangedMessage clickableChangeMsg = new ClickableChangedMessage();

        public Clickable currentClickable
        {
            get
            {
                if(clickables.Count == 0) return null;
                Clickable i = clickables[clickables.Count - 1];
                return i.isActiveAndEnabled ? i : null;
            }
        }

        public void AddClickable(Clickable clickable)
        {
            if (!clickables.Contains(clickable)) clickables.Add(clickable);
        }

        public void RemoveClickable(GameObject obj)
        {
            Clickable clickable = obj.GetComponent<Clickable>();
            if (clickable) RemoveClickable(clickable);
        }

        public void RemoveClickable(Clickable clickable)
        {
            bool isCurrent = clickable == currentClickable;
            if(clickables.Remove(clickable) && isCurrent)
            {
                if(clickables.Count > 0)
                {
                    clickable = clickables[clickables.Count - 1];
                    clickableChangeMsg.clickable = clickable;
                }
                else
                {
                    clickableChangeMsg.clickable = null;
                }

                MessageBuffer<ClickableChangedMessage>.Dispatch(clickableChangeMsg);
            }
        }

        protected virtual void OnDisable()
        {
            clickables.Clear();
            clickableChangeMsg.clickable = null;

            MessageBuffer<ClickableChangedMessage>.Dispatch(clickableChangeMsg);
        }

    }
}
