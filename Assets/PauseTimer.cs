using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PauseTimer : MonoBehaviour
    {
        void OnEnable()
        {
            Actions.PauseTime?.Invoke();
        }

        void OnDisable()
        {
            Actions.ResumeTime?.Invoke();
        }
    }
}
