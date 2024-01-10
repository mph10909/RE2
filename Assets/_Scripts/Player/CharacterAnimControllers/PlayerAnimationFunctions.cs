using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;

[System.Serializable]
public class BodyParts
{   
    [SerializeField]
    [Header("Body Parts")]
    public Transform head;
    public Transform torso;
    public Transform leg;

}


public class PlayerAnimationFunctions : MonoBehaviour
{
    
    [SerializeField] Transform player;
    [SerializeField] Transform foot;
    [SerializeField] GameObject splatter;
    [SerializeField] Collider[] colliders;
    [SerializeField] MonoBehaviour[] scripts;
    AudioSource audioSource;
    Animator anim;
    Rigidbody playerbody;
    RotationTest Rotate;
    [SerializeField] bool Deactivated;

    void Start()
    {
        Rotate = GetComponentInParent<RotationTest>();
        playerbody = GetComponentInParent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
    }


    void RotateStart()
    {
        StartCoroutine(Rotate.Rotate(0.35f));
    }

    void HeadStomp()
    {
        RaycastHit hit;
        Physics.SphereCast(foot.position, 2, Vector3.down, out hit, 2);
        if(hit.collider != null)
        {
            hit.transform.gameObject.GetComponent<IDamagable>().Damage
                (100, splatter, ZombieBody.Head, true, false, false, 0);
                
        }

    }

    private void EnableMovement()
    {
        if (Deactivated) return;
        Actions.DisableCharacterSwap?.Invoke(false);
        MovementBool(true);
    }

    private void DisableMovement()
    {
        if (Deactivated) return;
        Actions.DisableCharacterSwap?.Invoke(true);
        MovementBool(false);
    }

    private void MovementBool(bool enabled)
    {     
        foreach (MonoBehaviour script in scripts) script.enabled = enabled;
        transform.parent.gameObject.GetComponentInParent<NavMeshAgent>().enabled = enabled;
        foreach (Collider col in colliders) col.enabled = enabled;
    }
}
