using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class CubeMover : MonoBehaviour
{
    [SerializeField] float speed = 0.1f, backwardSpeed = 0.05f, sprintSpeed = 1.0f, rotate = 2.0f;

    PlayerAction playerActions;
    Vector2 moveInput;
    AnimationController trevorAnim;

    float currentSpeed;

    public float Speed
    {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }

    void Awake()
    {
        trevorAnim = GetComponent<AnimationController>();
        TutorialActionMediator.LocationChange += SetNewLocation;
        playerActions = new PlayerAction();
    }

    private void SetNewLocation(Transform location)
    {
        transform.position = location.position;
    }

    void OnEnable()
    {
        playerActions.Player.Enable();
    }

    void OnDisable()
    {
        playerActions.Player.Disable();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        float animSpeed = 0.0f;

        if (moveInput.y < 0)
        {
            Speed = backwardSpeed;
            animSpeed = -0.5f;
        }
        else if (moveInput.y == 0)
        {
            Speed = 0;
            animSpeed = 0.0f;
        }
        else if (moveInput.y > 0 && playerActions.Player.Sprint.ReadValue<float>() == 0)
        {
            Speed = speed;
            animSpeed = 0.5f;
        }
        else if (playerActions.Player.Sprint.ReadValue<float>() > 0)
        {
            Speed = sprintSpeed;
            animSpeed = 1.0f;
        }

        trevorAnim.UpdateAnimator(animSpeed);
    }

    void FixedUpdate()
    {
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        transform.Translate( 0, 0, Speed);
        transform.Rotate(0, moveInput.x * rotate, 0);
    }
}


