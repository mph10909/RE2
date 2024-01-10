
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class PistolFire : WeaponEffects, IFireable
{
    
    [SerializeField] Collider m_Collider;
    [SerializeField] GameObject splatters;

    const string AIMING = "Aiming";
    const string AIM = "Aim";
    const string FIRING = "Firing";
    const string FIRE = "Fire";

    [SerializeField] Transform playerHolding;
    RaycastHit hit;

    bool hitDetect;

    public override void OnEnable()
    {
        base.OnEnable();
        playerHolding = FindParentWithTag(this.transform, "Player");
    }

    public override void Firing()
    {
        FiringCheck();
    }

    private void FiringCheck()
    {
        if (anim.GetBool(FIRING) || anim.GetBool(RELOADING)) return;
        if (ammo.GetEmpty(ammoType) == 0 && ammo.GetStock(ammoType) == 0) { WeaponEmpty(); return; }
        if (ammo.GetEmpty(ammoType) == 0) { Reload(); return; }
        anim.SetTrigger(FIRE);
    }

    public bool Fire(AmmoInventory ammo)
    {
        int shotsFired = ammo.Spend(ammoType, ammoPerShot);
        return shotsFired > 0;
    }

    public override void WeaponFire()
    {
        print("Fired");

        if (isWeaponFireCalled) return;

        isWeaponFireCalled = true;

        Fire(ammo);
        WeaponFireEffects();
        CheckForHit();

        StartCoroutine(ResetFireFlag());
    }


    public override bool AIFire()
    {
        if (anim.GetBool(FIRING))
        {
            return false;
        }

        if (ammo.GetEmpty(ammoType) == 0 && ammo.GetStock(ammoType) == 0)
        {
            Actions.AmmoEmpty?.Invoke();
            return false;
        }

        if (ammo.GetEmpty(ammoType) == 0)
        {
            Reload();
            return false;
        }
        return true;
    }

    public override bool IsAmmoEmpty()
    {
        return ammo.GetEmpty(ammoType) <= 0 && ammo.GetStock(ammoType) <= 0;
    }

    void CheckForHit()
    {
        
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(m_Collider.bounds.center, weaponData.boxSize, -1 * transform.forward, transform.rotation, weaponData.m_MaxDistance, weaponData.shootMask);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.GetComponent<IDamagable>() != null)
            {
                ShootEnemy(hit);
                return;
            }

            if (hit.collider.GetComponent<IDestroyable>() != null)
            {
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
            enemy.Damage(weaponData.fowardDamage, splatters, weaponData.torso, weaponData.removeTorso, direction, weaponData.knockbackTorso, 0);
        }
        if (anim.GetFloat(AIM) > 0.1f)
        {
            enemy.Damage(weaponData.upWardDamage, splatters, weaponData.head, weaponData.removeHead, direction, weaponData.kockbackHead, 0);
        }
        if (anim.GetFloat(AIM) < -0.1f)
        {
            enemy.Damage(weaponData.downWardDamage, splatters, weaponData.legs, weaponData.removeLegs, direction, weaponData.knockbackLegs, 0);
        }

    }

    void Destroyable(RaycastHit hit)
    {
        IDestroyable destroyable = hit.collider.GetComponent<IDestroyable>();
        destroyable.Destroy();
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
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance * -1, weaponData.boxSize);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * weaponData.m_MaxDistance * -1);
            Gizmos.DrawWireCube(transform.position + transform.forward * weaponData.m_MaxDistance * -1, weaponData.boxSize);
        }
    }



}
