using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class ZombieSaveData : ID, ISaveable
    {
        private SaveManager _saveManager;
        public SaveManager SaveManager => _saveManager;


        [SerializeField] ZombieDamageController zombie;
        EnemyNavigation enemyNav;

        void Awake()
        {
            enemyNav = GetComponent<EnemyNavigation>();
        }

        public void OnCreated(SaveManager savemanager)
        {
            _saveManager = savemanager;
        }

        public void SaveData(SaveData saveData)
        {
            SaveData.EnemyData enemyData = new SaveData.EnemyData();
            enemyData.enemyID = UniqueID.ToString();
            enemyData.enemyName = this.gameObject.name.ToString();
            enemyData.enemyhealth = zombie.Health;
            enemyData.isDead = zombie.isDead;
            //enemyData.isEating = enemyNav.Eating;
            //print(enemyNav.Eating);
            saveData.enemyData.Add(enemyData);
            print(this.gameObject.name.ToString());
        }

        public void LoadData(SaveData saveData)
        {
            foreach (SaveData.EnemyData enemyData in saveData.enemyData)
            {
                if (enemyData.enemyName == UniqueID.ToString())
                {
                    //enemyNav.Eating = enemyData.isEating;
                    zombie.Health = enemyData.enemyhealth;
                    zombie.isDead = enemyData.isDead;
                    HealthTest();
                    break;
                }
            }

        }

        void HealthTest()
        {
            if (zombie.Health <= 0)
            {
                zombie.isDead = true;
                print("NoHealth");
                this.gameObject.SetActive(false);
                _saveManager.DestroyEnemy(this);
            }
        }
    }
}
