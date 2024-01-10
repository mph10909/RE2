
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;
using System;

public class Knife : WeaponEffects, IFireable
{
    
    [SerializeField] Collider m_Collider;
    [SerializeField] GameObject splatters;
    [SerializeField] LayerMask shootMask;

    [Header("Aiming Up")]
    [SerializeField] int upWardDamage = 25;
    [SerializeField] ZombieBody head;
    [SerializeField] bool removeHead;
    [SerializeField] bool kockbackHead;

    [Header("Aiming Forward")]
    [SerializeField] int fowardDamage = 25;
    [SerializeField] ZombieBody torso;
    [SerializeField] bool removeTorso;
    [SerializeField] bool knockbackTorso;

    [Header("Aiming Down")]
    [SerializeField] int  downWardDamage = 25;
    [SerializeField] ZombieBody legs;
    [SerializeField] bool removeLegs;
    [SerializeField] bool knockbackLegs;

    [Header("Weapon Damage / Range")]
    [SerializeField] Vector3 boxSize = new Vector3(3,3,3);
    [SerializeField] float m_MaxDistance = 20;
    [SerializeField] float minForce, maxForce;

    const string AIMING = "Aiming";
    const string AIM = "Aim";
    const string FIRING = "Firing";
    const string FIRE = "Fire";
    const string CURRENTCHARACTER = "CurrentChar";
    const string TWOHANDEDWEAPON = "TwoHandedWeapon";

    [SerializeField] Transform playerHolding;
    RaycastHit hit;
    bool hitDetect;
    private bool isCheckingHits = false;

    void OnDisable()
    {
        Actions.FiredWeapon -= Firing;
        anim.SetBool("Knife", false);
        anim.SetBool("NoWeapon", true);
        print(playerHolding.ToString() + " Knife Off");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Actions.FiredWeapon += Firing;
        playerHolding = FindParentWithTag(this.transform, "Player");
        anim = GetComponentInParent<Animator>();
        anim.SetBool("NoWeapon", false);
        anim.SetBool(TWOHANDEDWEAPON, false);
        anim.SetBool("Knife", true);
    }


    void Firing()
    {
        if (!anim.GetBool(CURRENTCHARACTER)) return;
        if (anim.GetBool(FIRING) || anim.GetBool(RELOADING)) return;
        anim.SetTrigger(FIRE);
        StartCoroutine(CheckHitsCoroutine());
    }

    IEnumerator CheckHitsCoroutine()
    {
        while (anim.GetBool("Firing"))
        {
            anim.GetBool("Firing");
            CheckForHit();
            yield return null;
        }
    }

    public void WeaponFire()
    { 
        WeaponFireEffects();
        //CheckForHit();
        StartCoroutine(CheckHitsCoroutine());
    }

    void CheckForHit()
    {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(m_Collider.bounds.center, boxSize, -1 * transform.forward, transform.rotation, m_MaxDistance, shootMask);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.GetComponent<IDamagable>() != null)
            {
                StopAllCoroutines();
                ShootEnemy(hit);
                return;
            }

            if (hit.collider.GetComponent<IDestroyable>() != null)
            {
                print("Knife Hit");
                Destroyable(hit);
            }
        }

    }

    void ShootEnemy(RaycastHit hit)
    {
        bool direction = IsEnemyFacing(playerHolding, hit.collider.transform);
        IDamagable enemy = hit.collider.GetComponent<IDamagable>();

        if (anim.GetFloat(AIM) < 0.1f && anim.GetFloat(AIM) > -0.1f)
        {
            enemy.Damage(fowardDamage, splatters, torso, removeTorso, direction, knockbackTorso, 0);
        }
        if (anim.GetFloat(AIM) > 0.1f)
        {
            enemy.Damage(upWardDamage, splatters, head, removeHead, direction, kockbackHead, 0);
        }
        if (anim.GetFloat(AIM) < -0.1f)
        {
            enemy.Damage(downWardDamage, splatters, legs, removeLegs, direction, knockbackLegs, 0);
        }

    }

    void Destroyable(RaycastHit hit)
    {
        IDestroyable destroyable = hit.collider.GetComponent<IDestroyable>();
        destroyable.Destroy();
    }

    public void Reload()
    {
        anim.SetTrigger(RELOAD);
        return;
    }

    public bool IsEnemyFacing(Transform Player, Transform Enemy)
    {
        float enemyAngle;
        enemyAngle = Vector3.Angle(Enemy.forward, Player.forward);
        if (enemyAngle < 90) return (false);
        else return (true);
    }

    void OnDrawGizmos()
    {
        if (hitDetect)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance * -1);
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance * -1, boxSize);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance * -1);
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance * -1, boxSize);
        }
    }

    public bool AIFire()
    {
        if (anim.GetBool(FIRING) || anim.GetBool(RELOADING))
        {
            return false;
        }

        return true;
    }

    public bool IsAmmoEmpty()
    {
        return false;
    }
}
