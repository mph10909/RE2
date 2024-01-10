using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationController : MonoBehaviour
{

    private Animator animator;
    int walking, aiming, weapon, firing, reloading, climbing, walkTrigger, ladderTrigger, climbingWeight;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        walking = Animator.StringToHash("Walking");
        weapon = Animator.StringToHash("Weapon");
        firing = Animator.StringToHash("Fire");
        reloading = Animator.StringToHash("Reload");
        climbing = Animator.StringToHash("Climbing");
        walkTrigger = Animator.StringToHash("isWalking");
        aiming = animator.GetLayerIndex("WeaponAim");
    }

    public Animator Anim { get { return animator; } set { animator = value; } }

    public float AimDirection {  get { return animator.GetFloat("AimDirection"); } set { animator.SetFloat("AimDirection", value, 0.1f, Time.deltaTime); } }

    public float ClimbOffTime
    {
        get
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == "Climbing");
            return clip.length;
        }

    }

    public bool Firing
    {
        get { return animator.GetBool("isFiring"); }
    }

    public bool IsReloading { get { return animator.GetBool("isReloading"); } }
        
    public bool IsClimbing{ get { return animator.GetBool("isClimbing"); } set { animator.SetBool("isClimbing", value); } }

    public float AimingWeight
    {
        get { return animator.GetLayerWeight(aiming); }
    }

    public float WeaponEquipped
    {
        get { return animator.GetFloat(weapon); }
    }

    public float Walking { get { return animator.GetFloat("Walkng"); }set { animator.SetFloat("Walkng", value); } }

    public void UpdateAnimator(float Movement)
    {
        if (Movement != 0)
        {
            animator.SetFloat(walking, Movement, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(walking, Mathf.Lerp(animator.GetFloat(walking), 0.001f, Time.deltaTime * 10f));
            if(animator.GetFloat(walking) < 0.05f) { animator.SetFloat(walking, 0); }
        }
    }

    public void UpdateAimDirection(float Direction)
    {
        if (Direction != 0)
        {
            AimDirection = Direction;
        }
        else
        {
            AimDirection = Mathf.Lerp(Direction, 0.001f, Time.deltaTime * 10f);
            if (AimDirection < 0.05f) { AimDirection = 0; }
        }
    }


    public void UpdateAiming(float isAiming)
    {
        if (WeaponEquipped == 0 || Firing) return;
        animator.SetLayerWeight(aiming, isAiming);

    }

    public void UpdateReloading()
    {
        animator.SetTrigger(reloading);
    }

    public void UpdateFiring()
    {
        animator.SetTrigger(firing);
    }

    public void UpdateClimbing(bool isClimbing, float Climbing)
    {
        if (Climbing != 0 && Climbing <= 1) animator.SetFloat(climbing, Climbing, 0.1f, Time.deltaTime);
        else if (Climbing == 2 && !isClimbing) animator.SetFloat(climbing, 2f);
        else animator.SetFloat(climbing, 0f);
    }

    public void SetClimbing(bool isClimbing)
    {
        IsClimbing = isClimbing;
    }

    public void SetWalking(bool isWalking)
    {
        if (isWalking)
        {
            animator.SetTrigger(walkTrigger);
            IsClimbing = false;
        }
        else
        {
            animator.ResetTrigger(walkTrigger);
        }
        

    }

}
