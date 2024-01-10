using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class ZombieDamageController : HealthManager, IDamagable, IComponentSavable
{

    [SerializeField] GameObject head;
    [SerializeField] GameObject leg;
    [SerializeField] GameObject torso;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bodyExplode;
    public bool isDead;


    const string HEALTH = "Health";
    const string CRAWLING = "Crawling";
    const string FORCE = "Force";

    public bool Damaged
    {
        get { return Anim.GetBool("BeenShot"); }
        set { Anim.SetBool("BeenShot", value); print(Damaged); }
    }



    public void Damage(int DamageAmount, GameObject Splatter, ZombieBody BodyPart, bool RemoveLimb, bool Facing, bool KnockBack, float Force)
    {

        Health = Mathf.Clamp(Health - DamageAmount, 0, 100);

        Damaged = true;
        Anim.SetBool("BeenShot", true);

        switch (BodyPart)
        {
            case ZombieBody.Head:
                if (RemoveLimb) { HeadRemoval(Splatter); return; }
                if (CheckIfDead(Facing)) { isDead = true; return; }
                break;
            case ZombieBody.Torso:
                if (Splatter != null) InstantiateTorsoGore(Splatter);
                if (CheckIfDead(Facing)) { isDead = true; return; }
                KnockedBack(Force, KnockBack, Facing);
                break;
            case ZombieBody.Legs:
                if (Splatter != null) InstantiateLegGore(Splatter);
                if (RemoveLimb) { LegRemoval(); return; }
                if (CheckIfDead(Facing)) { isDead = true; return; }
                KnockedBack(Force, KnockBack, Facing);
                break;
        }
    }

    void InstantiateHeadGore(GameObject splatter)
    {
        Instantiate(splatter, head.transform.position, head.transform.rotation);
    }

    void InstantiateLegGore(GameObject splatter)
    {
        Instantiate(splatter, leg.transform.position, leg.transform.rotation);
    }

    void InstantiateTorsoGore(GameObject splatter)
    {
        Instantiate(splatter, torso.transform.position, torso.transform.rotation);
    }

    void KnockedBack(float force, bool knockedBack, bool isFacing)
    {
        if (Anim.GetBool("LimbMissing")) return;
        print("Shouldnt Appear Unless Standing");
        print("Knocked");
        string trigger = "";
        if (isFacing && knockedBack) trigger = "FallBackward";
        if (!isFacing && knockedBack) trigger = "FallForward";
        if (isFacing && !knockedBack) trigger = "StumbleBack";
        if (!isFacing && !knockedBack) trigger = "StumbleForward";

        Anim.SetFloat(FORCE, force);
        Anim.SetTrigger(trigger);

        if (isFacing && !knockedBack)
        {
            DetectAndKnockBackOtherEnemies(force);
        }
    }

    private void DetectAndKnockBackOtherEnemies(float force)
    {
        print("Knock Back Other Enemies");

        float radius = 5.0f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.up);
        foreach (RaycastHit hit in hits)
        {
            ZombieDamageController otherEnemy = hit.collider.GetComponent<ZombieDamageController>();
            if (otherEnemy != null)
            {
                otherEnemy.KnockedBackByEnemy(force);
            }
        }
    }

    public void KnockedBackByEnemy(float force)
    {
        print("Knock Back Other Enemies");
        Anim.SetFloat(FORCE, force);
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            Anim.SetTrigger("StumbleBack");
        }
        else
        {
            Anim.SetTrigger("FallBackward");
        }
    }

    void HeadRemoval(GameObject splatter)
    {
        audioSource.PlayOneShot(bodyExplode);
        head.SetActive(false);
        InstantiateHeadGore(splatter);
        Anim.SetBool("HeadMissing", true);
        Anim.SetBool("LimbMissing", true);
        int randomTrigger = UnityEngine.Random.Range(0, 2);
        if (Anim.GetFloat("Crawling") < 1)
        {
            if (randomTrigger == 0)
            {
                Anim.SetTrigger("FallForward");
            }
            else
            {
                Anim.SetTrigger("FallBackward");
            }
        }

    }

    void LegRemoval()
    {
        Anim.SetFloat("MovementSpeed", 0);
        audioSource.PlayOneShot(bodyExplode);
        leg.SetActive(false);
        Anim.SetBool("LimbMissing", true);
        Anim.SetTrigger("FallForward");

    }

    bool CheckIfDead(bool isFacing)
    {
        if (Health > 0) return false;
        if (isFacing && !Anim.GetBool("LimbMissing")) Anim.SetTrigger("FallBackward");
        if (!isFacing && !Anim.GetBool("LimbMissing")) Anim.SetTrigger("FallForward");
        return true;
    }

    public string GetSavableData()
    {
        return Health.ToString();
    }

    public void SetFromSaveData(string savedData)
    {
        Health = (int)Convert.ToInt32(savedData);

        if(Health <= 0)
        {
            print("No Health");
            OnDeath?.Invoke();
        }
    }
}
