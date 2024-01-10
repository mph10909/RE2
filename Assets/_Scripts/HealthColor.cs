using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResidentEvilClone
{
    [Serializable]
    public class HealthColor
    {
        public enum Health
        {
            Green,
            Yellow,
            Red,
            Purple
        }

        public enum Condition
        {
            Fine,
            Caution,
            Danger,
            Posion
        }

        public Health healthColor;
        public Condition healthCondition;
        public bool poison;

        public Color HealthColors()
        {
            switch (healthColor)
            {
                default:
                    case Health.Green:  return new Color32(48, 235, 51, 255);
                    case Health.Yellow: return new Color32(241, 165, 31, 255);
                    case Health.Red:    return new Color32(236, 7, 2, 255);
                    case Health.Purple: return new Color32(145, 16, 210, 255);
            }
        }

        public Sprite HealthCondition()
        {
            switch (healthCondition)
            {
                default:
                    case Condition.Fine:    return ItemAssets.Instance.fine;
                    case Condition.Caution: return ItemAssets.Instance.caution;
                    case Condition.Danger:  return ItemAssets.Instance.danger;
                    case Condition.Posion:  return ItemAssets.Instance.poision;

            }
        }

    }
}
