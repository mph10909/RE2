using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RoomSaveData : MonoBehaviour, ISaveable
    {
        
        [SerializeField] Transform thisArea;
        Transform thisplayer;
        Collider roomCollider;
        [SerializeField] List<GameObject> _enemies;

        void OnEnable()
        {
            Actions.CharacterSwap += SetPlayer;
        }
        void OnDisable()
        {
            Actions.CharacterSwap -= SetPlayer;
        }

        void OnValidate()
        {
            thisArea = this.transform;
            
        }

        void Start()
        {
            _enemies = new List<GameObject>();
            TraverseChildren(transform);
            roomCollider = GetComponent<Collider>();
            //ActivateEnemy(RoomName());
        }

        void TraverseChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.GetComponent<EnemyNavigation>() != null)
                {
                    //Debug.Log("Child name: " + child.name);
                    _enemies.Add(child.gameObject);
                }

                TraverseChildren(child);
            }
        }

        public void SaveData(SaveData saveData)
        {
            if (!RoomName()) return;
            saveData.roomName = thisArea.name;           
        }

        public void LoadData(SaveData saveData)
        {

        }

        void SetPlayer(GameObject player)
        {
            thisplayer = player.transform;
            //ActivateEnemy(RoomName());
        }

        bool RoomName()
        {
            if (roomCollider == null) return false;
            if (roomCollider.bounds.Contains(thisplayer.position)) return true;
            else return false;        
        }

        void ActivateEnemy(bool playerPresent)
        {
            print(playerPresent);
            foreach(GameObject enemy in _enemies)
            {
                enemy.SetActive(playerPresent);
            }
        }

    
    }
}
