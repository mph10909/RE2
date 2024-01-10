using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCollision : MonoBehaviour
{

    NavMeshAgent _player;

    void Start()
    {
        _player = GetComponent<NavMeshAgent>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            _player.isStopped = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            _player.isStopped = true;
        }
    }
}
