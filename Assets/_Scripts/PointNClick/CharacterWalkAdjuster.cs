using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterWalkAdjuster : MonoBehaviour
{
    PointAndClick characterController;
    HealthManager health;
    NavMeshAgent Player;
    Animator anim;
    float movementSpeed;
    bool isRunning;

    [Header("Walking Speeds")]
    [SerializeField][Range(0, 20)] int greenWalking;
    [SerializeField][Range(0, 20)] int yellowWalking;
    [SerializeField][Range(0, 20)] int redWalking;
    [Header("Running Speeds")]
    [SerializeField][Range(0, 20)] int greenRunning;
    [SerializeField][Range(0, 20)] int yellowRunning;
    [SerializeField][Range(0, 20)] int redRunning;

    public PointAndClick CharacterController { get { return characterController; } }
    public bool Sprint { set { isRunning = value; } }


    void Start()
    {
        characterController = GetComponent<PointAndClick>();
        health = GetComponent<HealthManager>();
        Player = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        Player.speed = greenWalking;
    }

    void Update()
    {
        if (isRunning) RunSpeed();
        if (!isRunning) WalkSpeed();
        Running();
        movementSpeed = Mathf.Abs(Player.velocity.x);
        anim.SetFloat(AnimValue.Speed, movementSpeed);
        if (movementSpeed == 0.0f)
        {
            anim.SetBool(AnimValue.HasNoMovement, true);
        }
        else anim.SetBool(AnimValue.HasNoMovement, false);
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
                Player.speed = redWalking;
                break;
            case int i when i > 35 && i <= 65:
                Player.speed = yellowWalking;
                break;
            default:
                Player.speed = greenWalking;
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
                Player.speed = redRunning;
                    break;
                case int i when i > 35 && i <= 65:
                Player.speed = yellowRunning;
                    break;
                default:
                    Player.speed = greenRunning;
                    break;
            }

        }
    }


    void Running()
    {
        if (CharacterController.IsSprinting)
        {
            isRunning = true;
            if (anim.GetBool(AnimValue.IsTwoHanded)) anim.SetFloat(AnimValue.Running, 2.0f);
            else anim.SetFloat(AnimValue.Running, 1.0f);
        }
        else
        {
            isRunning = false;
            anim.SetFloat(AnimValue.Running, 0.0f);
        }
    }


}
