using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HealthManager, IDestroyable
    {


    public void Destroy()
    {
        DamageHealth(10);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(Health <= 0)
        {
            this.gameObject.SetActive(false);

        }
    }

    void OnDisable()
        {
            Actions.EnemyKilled?.Invoke();
            Destroy(this.gameObject);
        }
    }

