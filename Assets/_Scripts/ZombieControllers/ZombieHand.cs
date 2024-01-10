using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHand : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform player;
    [SerializeField] Transform enemy, hand;
    [SerializeField] float groundDistance = 7;
    [SerializeField] int damage;
    [SerializeField] bool attacking;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    void OnTriggerStay(Collider col)
    {

        if (attacking) return;
        if (col.transform.gameObject.tag == "Player") player = col.transform.gameObject.transform;
        if (col.transform.gameObject.tag == "Player" && anim.GetFloat("Distance") < groundDistance)
        {
            bool facingDir = IsPlayerFacing();
            print(IsPlayerFacing());
            if (anim.GetBool("Standing"))
            {
                StartCoroutine(AttackCoroutine(AttackArea.UpperFront, AttackArea.UpperBack, facingDir, player, "Upper"));
            }
            else
            {
                StartCoroutine(AttackCoroutine(AttackArea.LowerFront, AttackArea.LowerBack, facingDir, player, "Ground"));
            }
        }
    }

    IEnumerator AttackCoroutine(AttackArea frontAttack, AttackArea backAttack, bool facingDir, Transform target, string attack)
    {
        attacking = true;
        if (facingDir) target.GetComponent<IAttackable>().Attacked(enemy, frontAttack, damage);
        if (!facingDir) target.GetComponent<IAttackable>().Attacked(enemy, backAttack, damage);
        anim.SetTrigger(attack + "Attack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        attacking = false;
    }


    public bool IsPlayerFacing()
    {
        Vector3 enemyDir = transform.position - player.transform.position;
        Vector3 playerDir = player.transform.position - transform.position;
        float playerAngle = Vector3.Angle(player.transform.forward, enemyDir);
        if (playerAngle < 90) return true;
        else return false;
    }


    //void OnTriggerStay(Collider col)
    //{
    //    if (attacking) return;
    //    if (col.transform.gameObject.tag == "Player") player = col.transform.gameObject.transform;
    //    if (col.transform.gameObject.tag == "Player" && anim.GetFloat("Distance") < groundDistance && !anim.GetBool("Standing"))
    //    {
    //        print("LowerAttack");
    //        attacking = true;
    //        bool facingDir = IsPlayerFacing();
    //        anim.SetTrigger("GroundAttack");
    //        IAttackable attack = col.transform.gameObject.GetComponent<IAttackable>();
    //        if (facingDir) { attack.Attacked(enemy, AttackArea.LowerFront, 0); return; }
    //        if (!facingDir) { attack.Attacked(enemy, AttackArea.LowerBack, 0); return; }
    //    }

    //    if (col.transform.gameObject.tag == "Player" && anim.GetFloat("Distance") < groundDistance && anim.GetBool("Standing"))
    //    {
    //        print("UpperAttack");
    //        attacking = true;
    //        bool facingDir = IsPlayerFacing();
    //        anim.SetTrigger("UpperAttack");
    //        IAttackable attack = col.transform.gameObject.GetComponent<IAttackable>();
    //        if (facingDir) { attack.Attacked(enemy, AttackArea.UpperFront, damage); }
    //        if (!facingDir) { attack.Attacked(enemy, AttackArea.UpperBack, damage); }
    //    }

    //}

    //void OnTriggerExit(Collider col)
    //{
    //    attacking = false;
    //}

    //public bool IsPlayerFacing()
    //{
    //    Vector3 enemyDir;
    //    Vector3 playerDir;
    //    float playerAngle;
    //    enemyDir = transform.position - player.transform.position;
    //    playerDir = player.transform.position - enemy.transform.position;
    //    playerAngle = Vector3.Angle(player.transform.forward, enemyDir);
    //    if (playerAngle < 90) return (true);
    //    else return (false);
    //}



}
