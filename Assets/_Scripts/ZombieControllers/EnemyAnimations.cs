using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;
using System;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] AnimatorScript startingAnim;
    [SerializeField] float armsUpSpeed;
    [SerializeField] float armDistance;
    [SerializeField] float armFloat = 0;
    [SerializeField] bool playerDead;
    [SerializeField] bool isEating;

    [HideInInspector] public NavMeshAgent enemy;

    Animator         anim;
    EnemyWalkSoundFX enemyFX;
    public GameObject character;

    float playerDistance;

    const string ARMSOUT = "ArmOut";
    const string MOVING = "Moving";
    const string LIMBMISSING = "LimbMissing";
    const string CRAWLING = "Crawling";
    const string HEADMISSING = "HeadMissing";
    const string DISTANCE = "Distance";

    public Animator EnemyAnimator
        {
            get { return anim; }
            set { anim = value; }
        }

    public float PlayerDistance
    {
        get { return playerDistance; }
        set { playerDistance = value; }
    }

    public float ArmDistance { get { return armDistance; } }

    public float ArmFloat
    {
        get { return armFloat; }
        set { armFloat = Mathf.Clamp(value, 0, 1); }
    }

    public float ArmsUpSpeed { get { return armsUpSpeed; } }

    void OnDisable()
    {
        EnemyEventHandler.UnregisterEnemy(this);
    }

    void Awake()
    {
        EnemyEventHandler.RegisterEnemy(this);
        enemyFX = GetComponent<EnemyWalkSoundFX>();
        enemy = GetComponent<NavMeshAgent>();
        enemy.speed = UnityEngine.Random.Range(2.75f, 3.75f);
        anim = GetComponentInChildren<Animator>();

        anim.Play(startingAnim.Hash);

        if (isEating)
        {
            EnemyAnimator.SetBool("Eating", true);
            EnemyAnimator.SetTrigger("SetEating");
        }


    }

    void Update()
    {
        if(!isEating) EnemyAnimator.SetBool("Eating", false);
        float movementSpeed;

        movementSpeed = Mathf.Abs(enemy.velocity.x);
        anim.SetFloat("MovementSpeed", movementSpeed);

        PlayerDistance = Vector3.Distance(this.transform.position, character.transform.position);
        anim.SetFloat(DISTANCE, PlayerDistance);

        EnemyFX();
        ArmsUp();
        PlayerKilled(playerDead, character);
    }

    void EnemyFX()
    {
        if(anim.GetBool(MOVING)) enemyFX.enabled = true;
        else enemyFX.enabled = false;
    }

    void ArmsUp()
    {
        if (anim.GetBool(LIMBMISSING)) {anim.SetFloat("WalkingSelection", 0.0f);}

        if (!anim.GetBool(LIMBMISSING))
        {
            if (PlayerDistance < ArmDistance)
            {
                ArmFloat = ArmFloat + ArmsUpSpeed * Time.deltaTime;
                anim.SetFloat(ARMSOUT, Mathf.Clamp(ArmFloat, 0, 1));
            }
            else
            {
                ArmFloat = ArmFloat - 3f * Time.deltaTime;
                anim.SetFloat(ARMSOUT, Mathf.Clamp(ArmFloat, 0, 1));
            }
        }

    }

    public void PlayerKilled(bool death, GameObject character)
    {
        playerDead = death;
        if (!playerDead) return;
        if (EnemyAnimator.GetFloat(DISTANCE) < 6.0f)
        {
            print("character Has Died");
            anim.SetTrigger("PlayerDied");
            playerDead = false;
        }
    }

    public void SetCharacter(GameObject newCharacter)
    {
        character = newCharacter;
    }
}
