using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCheck : MonoBehaviour
{
    [SerializeField] Collider m_Collider;
    [SerializeField] LayerMask shootMask;
    [SerializeField] Vector3 boxSize = new Vector3(3, 3, 3);
    [SerializeField] float m_MaxDistance = 20;
    bool hitDetect;
    RaycastHit hit;
   
   public void CheckForHit()
        
    {
        
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(m_Collider.bounds.center, boxSize, transform.forward, transform.rotation, m_MaxDistance, shootMask);
        foreach (RaycastHit hit in hits)
        {
            
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                print("check interactable");
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                interactable.Interact();
            }
        }
       
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForHit();
        }
    }
    void OnDrawGizmos()
    {
        if (hitDetect)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(m_Collider.bounds.center, transform.forward * hit.distance);
            Gizmos.DrawWireCube(m_Collider.bounds.center + transform.forward * hit.distance, boxSize);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(m_Collider.bounds.center, transform.forward * m_MaxDistance);
            Gizmos.DrawWireCube(m_Collider.bounds.center + transform.forward * m_MaxDistance, boxSize);
        }
    }
}
