using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class PlayerAttacked : HealthManager, IAttackable, IComponentSavable
{
    public HealthColor healthStats;
    [SerializeField] GameObject splatter;
    [SerializeField] AudioClip deathMoan;

    const string ATTACKED = "Attacked";

    const string FRONTLOWERATTACK = "FrontLowerAttack";
    const string BACKLOWERATTACK = "BackLowerAttack";

    const string BACKATTACK = "BackAttack";
    const string FRONTATTACK = "FrontAttack";

    void OnValidate()
    {
        SetAnimHealth();
    }

    public void Attacked(Transform EnemyAttacking, AttackArea BodySection, int Damage)
    {
        Actions.PathIsFinished?.Invoke();
        DamageHealth(Damage);
        Death();

        switch (BodySection)
        {
            case AttackArea.UpperFront:
                HandleAttack(EnemyAttacking, FRONTATTACK);
                break;
            case AttackArea.LowerFront:
                HandleAttack(EnemyAttacking, FRONTLOWERATTACK);
                break;
            case AttackArea.UpperBack:
                HandleAttack(EnemyAttacking, BACKATTACK);
                break;
            case AttackArea.LowerBack:
                HandleAttack(EnemyAttacking, BACKLOWERATTACK);
                break;
        }

        SetHealthStats();
    }

    void HandleAttack(Transform enemy, string trigger)
    {
        Anim.SetBool("Firing", false);
        if (trigger == BACKLOWERATTACK || trigger == BACKATTACK)
        {
            Vector3 dir = transform.position - enemy.position;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, 180, 0));
        }
        else
        {
            transform.LookAt(enemy);
        }
        Anim.ResetTrigger("ReloadTrigger");
        Anim.ResetTrigger("Fire");
        Anim.SetTrigger(trigger);
        Anim.SetTrigger(ATTACKED);


    }

    public void SetHealthStats()
    {
        if (Health >= 65)
        {
            healthStats.healthColor = HealthColor.Health.Green;
            healthStats.healthCondition = HealthColor.Condition.Fine;
        }
        else if (Health >= 35)
        {
            healthStats.healthColor = HealthColor.Health.Yellow;
            healthStats.healthCondition = HealthColor.Condition.Caution;
        }
        else
        {
            healthStats.healthColor = HealthColor.Health.Red;
            healthStats.healthCondition = HealthColor.Condition.Danger;
        }
    }

    void Death()
    {
        if(Health <= 0)
        {
            Actions.OnPlayerKilled?.Invoke(true, this.gameObject);
            SoundManagement.Instance.PlaySound(deathMoan);
        }
    }

    public string GetSavableData()
    {
        return Health.ToString();
    }

    public void SetFromSaveData(string savedData)
    {
        Health = (int)Convert.ToInt32(savedData);
    }
}
