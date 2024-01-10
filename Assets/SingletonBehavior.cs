using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T m_Instance;

        public static bool Exists { get { return m_Instance != null; } }

        protected SingletonBehavior() { }

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = (T)FindObjectOfType(typeof(T), true);


                return m_Instance;
            }
        }


        protected virtual void Awake()
        {
            if (m_Instance == null)
                m_Instance = this as T;
        }
    }
}
