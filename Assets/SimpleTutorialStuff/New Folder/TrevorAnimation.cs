using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrevorAnimation : MonoBehaviour
{
    private Animator animator;
    int walking;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        walking = Animator.StringToHash("Walking");
    }

    public void UpdateAnimator(float Movement)
    {
        animator.SetFloat(walking, Movement, 0.1f, Time.deltaTime);
    }
}

