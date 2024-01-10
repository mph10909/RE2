using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class EnemyFacing : MonoBehaviour
    {
        [SerializeField] GameObject Player;
        [SerializeField] GameObject ThisEnemy;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

            float enemyAngle;
            enemyAngle = Vector3.Angle(ThisEnemy.transform.forward, Player.transform.forward);
            if (enemyAngle < 90) print("TRUE");
            else print("FALSE");

    }
    }
}
