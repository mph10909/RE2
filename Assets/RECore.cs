using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RECore : MonoBehaviour
    {
        [SerializeField] GameObject[] UIPrefab;
        [SerializeField] Transform UITransform;

        // Start is called before the first frame update
        void Awake()
        {
            RECore[] cores = FindObjectsOfType<RECore>();
            if (cores.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);


        }

        private void Start()
        {
            foreach(GameObject UI in UIPrefab)
            {
                Instantiate(UI, UITransform);
            }
        }

    }
}
