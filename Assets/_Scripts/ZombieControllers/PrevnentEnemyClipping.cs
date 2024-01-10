using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{
    public class PrevnentEnemyClipping : MonoBehaviour
    {
        [SerializeField] LayerMask layer;
        [SerializeField] NavMeshAgent nav;

        void OnTriggerEnter(Collider col)
        {
            if(layer == (layer | (1 << col.gameObject.layer)))
            {
                nav.enabled = true;
            }
        }
    }
}
