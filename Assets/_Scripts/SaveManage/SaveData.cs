using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    
    public class SaveData
    {
        public string sceneName;

        public int sceneInt;

        public int saveAmount;

        public int firstAidUsed;

        public string audioName;

        public string activePlayer;

        public int movableCharacter;

        public Character character;

        public string roomName;

        public float timePlayed;

        public List<FileData> inventoryFiles;
        public List<GameObject> availableCharacters;


        [System.Serializable]
        public struct CharacterData
        {

            public string playerName;

            public Vector3 position;
            public Quaternion rotation;

            public float audioLevel;

            public int playerHealth;

            public Item weaponEquipped;

            public bool aim;
            public bool weapon;
            public bool active;

            public int weaponequipped;

            public List<Item> inventory;

            public List<int> stockedAmmo;
            public List<int> loadedAmmo;

        }

        [System.Serializable]
        public class ItemData
        {   
            public string name;
            public int amount;
            public bool active;
        }

        [System.Serializable]
        public struct EnemyData
        {
            
            public string enemyName;
            public int enemyhealth;
            public bool isDead;
            public bool isEating;
            public string enemyID;
        }

        [System.Serializable]
        public struct CameraData
        {
            public string name;
            public Vector3 position;
            public Quaternion rotation;
            public bool enabled;
        }

        [System.Serializable]
        public struct RoomData
        {
        }

        [System.Serializable]
        public struct BooleanData
        {
            public string objName;
            public GameObject obj;
            public List<string> trueBools;
            public List<string> falseBools;
        }

        [System.Serializable]
        public struct StorageData
        {
            public string name;
            public Item item;
        }

        public List<BooleanData> booleanData = new List<BooleanData>();
        public List<ItemData> itemData = new List<ItemData>();
        public List<CameraData> cameraData = new List<CameraData>();
        public List<CharacterData> characterData = new List<CharacterData>();
        public List<EnemyData> enemyData = new List<EnemyData>();
        public List<RoomData> roomData = new List<RoomData>();
        public List<StorageData> storageData = new List<StorageData>();


        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);
        }
    }

    public interface ISaveable
    {
        void SaveData(SaveData a_SaveData);
        void LoadData(SaveData a_SaveData);
    }

}
