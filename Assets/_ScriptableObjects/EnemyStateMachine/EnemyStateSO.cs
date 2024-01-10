using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    public abstract class EnemyStateSO : ScriptableObject
    {
        public abstract void OnStateEnter(EnemyAI enemy);
        public abstract void Tick(EnemyAI enemy);
        public abstract void OnStateExit(EnemyAI enemy);
    }
}
