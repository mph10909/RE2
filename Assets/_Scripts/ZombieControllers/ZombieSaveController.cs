using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ZombieSaveController : MonoBehaviour, ISaveable
    {
        private SaveManager _saveManager;
        public SaveManager SaveManger => _saveManager;

        [SerializeField] ZombieDamageController health;

        public void OnCreated(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public void SaveData(SaveData saveData)
        {
            SaveData.EnemyData enemyData = new SaveData.EnemyData();
            enemyData.enemyName = this.gameObject.name.ToString();
            enemyData.enemyhealth = health.Health;
            saveData.enemyData.Add(enemyData);

        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.EnemyData enemyData in saveData.enemyData)
            {
                if (enemyData.enemyName == this.gameObject.name.ToString())
                {
                    health.Health = enemyData.enemyhealth;
                    break;
                }
            }

            if (health.Health <= 0)
            {
                //_saveManager.DestroyEnemy(this);
            }
        }
    }
}
