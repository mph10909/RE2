using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingEnemyAngle : MonoBehaviour
{
    GameObject character;

    private void Awake()
    {
        Actions.CharacterSwap += SetCharacter;
    }

    private void OnDestroy()
    {
        Actions.CharacterSwap -= SetCharacter;
    }

    public bool IsPlayerFacing()
    {
        Vector3 enemyDir = (transform.position - character.transform.position).normalized;
        float dotProduct = Vector3.Dot(character.transform.forward, enemyDir);

        // If dot product is positive, it means player is facing the enemy.
        return dotProduct > 0;
    }

    //public bool IsPlayerFacing()
    //{
    //    Vector3 enemyDir;
    //    Vector3 playerDir;
    //    float playerAngle;
    //    enemyDir = transform.position - character.transform.position;
    //    playerDir = character.transform.position - transform.position;
    //    playerAngle = Vector3.Angle(character.transform.forward, enemyDir);
    //    if (playerAngle < 90) return (true);
    //    else return (false);
    //}

    void SetCharacter(GameObject newCharacter)
    {
        character = newCharacter;
    }
}
