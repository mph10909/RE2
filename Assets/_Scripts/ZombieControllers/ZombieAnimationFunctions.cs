using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimationFunctions : MonoBehaviour
{
    NavMeshAgent navMesh;
    Animator anim;
    Rigidbody enemy;

    [SerializeField] CapsuleCollider enemyCapsule;
    [SerializeField] Collider[] colliders;
    [SerializeField] MonoBehaviour[] scripts;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        enemy = GetComponentInParent<Rigidbody>();

        navMesh = GetComponentInParent<NavMeshAgent>();
    }

    public void HasBeenShot()
    {
        print("Shot");
        //anim = GetComponent<Animator>();
        anim.SetBool("BeenShot", true);
    } 

    void Standing(bool standing)
    {
        anim.SetBool("Standing", standing);
    }

    void EnableFallBack()
    {
        Standing(false);
        DisableMovent();
        float forceAmount = anim.GetFloat("Force");
        enemy.AddForce(transform.forward * forceAmount, ForceMode.Force);
    }

    void EnableKnockBack()
    {
        DisableMovent();
        float forceAmount = anim.GetFloat("Force");
        enemy.AddForce(transform.forward * forceAmount, ForceMode.Force);
    }

    void EnableFallForward()
    {
        Standing(false);
        DisableMovent();
        float forceAmount = anim.GetFloat("Force");
        enemy.AddForce(-transform.forward * forceAmount, ForceMode.Force);
    }

    void DisableMovent()
    {
        navMesh.enabled = false;
        MovementBool(false);
    }

    void SetCrawl()
    {
        if (anim.GetBool("LimbMissing"))
        {
            anim.SetFloat("Crawling", 1);
        }
        if(enemyCapsule != null)
        {
            enemyCapsule.direction = 2;
            enemyCapsule.center = new Vector3(0, .1f, -.1f);
            print("Rotate Capsule");
        }

    }

    void DisableFalling()
    { 
        enemy.velocity = Vector3.zero;
        enemy.angularVelocity = Vector3.zero;
    }

    void HasBeenKilled()
    {
        MovementBool(false);
        navMesh.enabled = false;
    }

    void EnableWalk()
    {   
        anim.SetFloat("Force", 0);
        EnableMovement();
        Standing(true);
    }

    void EnableCrawl()
    {
        anim.SetFloat("Force", 0);
        EnableMovement();
        Standing(false);
        foreach(Collider collider in colliders)
        {
            var capsule = GetComponent<CapsuleCollider>();
            if(capsule != null)
            {
                capsule.direction = 2;
            }
        }
    }

    void EnableMovement()
    {
        navMesh.enabled = true;
        MovementBool(true);
    }

    void MovementBool(bool enabled)
    {
        foreach (MonoBehaviour script in scripts) script.enabled = enabled;
        foreach (Collider col in colliders) col.enabled = enabled;
        if (!navMesh.enabled) return;
        navMesh.isStopped = true;
    }
}
