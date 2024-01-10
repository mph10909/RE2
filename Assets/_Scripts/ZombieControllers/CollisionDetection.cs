using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Animator _playerAnim;
    [SerializeField] Animator _enemyAnim;
    [SerializeField] Transform _enemy;

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;
        if (_enemyAnim.GetInteger("Health") <= 0) return;
        if(collision.rigidbody)
        {
            //collision.gameObject.transform.LookAt(_enemy);
            _playerAnim = collision.gameObject.GetComponentInChildren<Animator>();
            _enemyAnim.SetTrigger("UpperAttack");
            _playerAnim.SetTrigger("AttackedFront");
            Debug.Log("PlayerCollision");
        }
    }

}
