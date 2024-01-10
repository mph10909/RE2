using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [Range(0, 100)][SerializeField] int healthAmount;
    Animator anim;
    const string CURRENTCHARTER = "CurrentChar";
    public UnityEvent OnDeath;

    public int Health
    {
        get { return healthAmount; }
        set { healthAmount = value; SetAnimHealth(); }
    }

    public Animator Anim
    {
        get { return anim; }
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        SetAnimHealth();
    }

    public void DamageHealth(int Damage)
    {
        Health = Mathf.Clamp(Health - Damage, 0, 100);
        SetAnimHealth();
    }

    public void SetAnimHealth()
    {
        
        if(Anim !=null) Anim.SetFloat("Health", Health);
        
    }

    public void Heal(int health)
    {
        if(anim.GetBool(CURRENTCHARTER))
        print("Healed + " + health);
        Health = Mathf.Clamp(Health + health, 0, 100);
        SetAnimHealth();
    }
}
