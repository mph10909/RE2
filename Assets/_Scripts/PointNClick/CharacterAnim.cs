using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnim : MonoBehaviour
{
    HealthManager health;
    NavMeshAgent Player;
    Animator anim;
    float movementSpeed;
    float playerSpeed;
    bool isRunning;

    void Start()
    {
        health = GetComponent<HealthManager>();
        Player = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        playerSpeed = Player.speed;
    }

    void Update()
    {
        if (isRunning) RunSpeed();
        if (!isRunning)WalkSpeed();
        Running();
        movementSpeed = Mathf.Abs(Player.velocity.x);
        anim.SetFloat("MovementSpeed", movementSpeed);
        if (movementSpeed == 0.0f)
        {
            anim.SetBool("NoMovement", true);
        }
        else anim.SetBool("NoMovement", false);
    }

    void WalkSpeed()
    {
        
        if (health != null)
        {
        switch (health.Health)
            {
            case 0:
                Player.speed = 0.0f;
                break;
            case int i when i > 0 && i <= 35:
                Player.speed = 4.5f;
                break;
            case int i when i > 35 && i <= 65:
                Player.speed = 7;
                break;
            default:
                Player.speed = playerSpeed;
                break;
            }    

        }
    }

    void RunSpeed()
    {
        if (health != null)
        {
            switch (health.Health)
            {
                case 0:
                    Player.speed = 0.0f;
                    break;
                case int i when i > 0 && i <= 35:
                Player.speed = 7f;
                    break;
                case int i when i > 35 && i <= 65:
                Player.speed = 10f;
                    break;
                default:
                    Player.speed = 20;
                    break;
            }

        }
    }


    void Running()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isRunning = true;
            anim.SetBool("Running", true);
        }
        else
        {
            isRunning = false;
            anim.SetBool("Running", false);
        }
    }
}
