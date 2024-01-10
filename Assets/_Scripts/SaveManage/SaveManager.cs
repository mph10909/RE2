using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

namespace ResidentEvilClone
{
    public class SaveManager : MonoBehaviour, ISaveable, IIncreasable
    {
        [SerializeField] int saveAmount;
        [SerializeField] int firstAidSpray;
        [SerializeField] FileMenuInventory inventoryFiles;
        
        [SerializeField] List<ZombieSaveData> enemyZombie = new List<ZombieSaveData>();
        [SerializeField] HashSet<ZombieSaveData> killedEnemy = new HashSet<ZombieSaveData>();  
        [SerializeField] List<string> killedEnemyID = new List<string>();

        List<CharacterSaveData> characterData = new List<CharacterSaveData>();
        List<CameraSaveData> cameraData = new List<CameraSaveData>();
        [SerializeField] List<ItemSaveData> itemData = new List<ItemSaveData>();
        [SerializeField] List<RoomSaveData> roomData = new List<RoomSaveData>();
        [SerializeField] List<StorageSaveData> storageData = new List<StorageSaveData>();
        [SerializeField] List<string> itemID = new List<string>();
        [SerializeField] CharacterManager characterManager;
        [SerializeField] List<BooleanSaveData> booleanData;
        [SerializeField] TimeManager pauseTimer;

        public int SaveAmount { get { return saveAmount; } }

        void Start()
        {

            itemData = new List<ItemSaveData>(FindObjectsOfType<ItemSaveData>());
            characterData = new List<CharacterSaveData>(FindObjectsOfType<CharacterSaveData>());
            enemyZombie = new List<ZombieSaveData>(Resources.FindObjectsOfTypeAll<ZombieSaveData>());
            cameraData = new List<CameraSaveData>(Resources.FindObjectsOfTypeAll<CameraSaveData>());
            roomData = new List<RoomSaveData>(FindObjectsOfType<RoomSaveData>());
            storageData = new List<StorageSaveData>(Resources.FindObjectsOfTypeAll<StorageSaveData>());
            characterManager = FindObjectOfType<CharacterManager>();
            booleanData = new List<BooleanSaveData>(Resources.FindObjectsOfTypeAll<BooleanSaveData>());
            pauseTimer = FindObjectOfType<TimeManager>();

            foreach (ZombieSaveData enemy in enemyZombie)
            {
                enemy.OnCreated(this);
            }

            if (Loader.Loaded) { print("LoadedStart"); LoadJsonData(Loader.SaveFile ,this); }

            foreach (CharacterSaveData character in characterData)
            {
                character.OnCreated();
            }
        }



        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SaveJsonData("Save1", this);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SaveJsonData("Save2", this);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SaveJsonData("Save3", this);

            }

        }

        public void TyperWriterSave(int save)
        {
            SaveJsonData("Save" + save, this);
        }


        void LateUpdate()
        {
            foreach (ZombieSaveData killedZombie in killedEnemy)
            {
                killedEnemyID.Add(killedZombie.gameObject.name);
                enemyZombie.Remove(killedZombie);
            }
            killedEnemy.Clear();

        }

        void LoadClear()
        {
            foreach (ZombieSaveData killedZombie in killedEnemy)
            {
                Destroy(killedZombie.gameObject);
            }
            killedEnemy.Clear();
        }

        public void DestroyEnemy(ZombieSaveData enemy)
        {
            killedEnemy.Add(enemy);
        }

        void SaveJsonData(string dataName, SaveManager saveManager)
        {    
            SaveData saveDatas = new SaveData();
            saveManager.SaveData(saveDatas);

            if (FileManager.WriteToFile(dataName + ".dat", saveDatas.ToJson()))
            {
                print("SaveSuccessful");
            }
        }

        public void SaveData(SaveData saveData)
        {
            SaveCharacters(saveData);
            SaveEnemies(saveData);
            SaveItems(saveData);
            SaveCameras(saveData);
            SaveRoom(saveData);
            SaveStorage(saveData);
            SaveBoolean(saveData);

            foreach (string enemyName in killedEnemyID)
            {
                SaveData.EnemyData enemyData = new SaveData.EnemyData();
                enemyData.enemyhealth = 0;
                enemyData.enemyName = enemyName;
                saveData.enemyData.Add(enemyData);
            }

            Loader.SaveAmount = Loader.SaveAmount + 1;

            saveData.saveAmount = Loader.SaveAmount;
            saveData.firstAidUsed = firstAidSpray;
            print(saveData.saveAmount);
            saveData.character = characterManager.characterName;
            saveData.movableCharacter = characterManager.moveableCharacter;
            saveData.availableCharacters = characterManager.characters;
            saveData.sceneInt = SceneManager.GetActiveScene().buildIndex;
            saveData.sceneName = SceneManager.GetActiveScene().name;
            saveData.audioName = SoundManagement.Instance.GetTrack();
            saveData.inventoryFiles = inventoryFiles.fileInventory.fileList;
            saveData.timePlayed = pauseTimer.elapsedTime;
        }

        public void LoadJsonData(string loadFile, SaveManager saveManager)
        {
           
            if (FileManager.LoadFromFile(loadFile + ".dat", out var json))
            {
                SaveData saveDatas = new SaveData();
                saveDatas.LoadFromJson(json);

                saveManager.LoadData(saveDatas);
                print("LoadSuccessful");
            }

        }

        public static SaveData GetData(string saveFile)
        {
            SaveData saveDatas = new SaveData();
            if (FileManager.LoadFromFile(saveFile + ".dat", out var json))
            { 
                saveDatas.LoadFromJson(json);   
            }
            return saveDatas;
            
        }

        public void LoadData(SaveData saveData)
        {
            firstAidSpray = saveData.firstAidUsed;
            if(pauseTimer != null)pauseTimer.elapsedTime = saveData.timePlayed;

            if(inventoryFiles != null) inventoryFiles.fileInventory.fileList = saveData.inventoryFiles;
            if (characterManager != null)
            {
                characterManager.characters = saveData.availableCharacters;
                CharacterLoader(saveData);
            }

            foreach (ZombieSaveData zombie in enemyZombie)
            {
                zombie.LoadData(saveData);
            }

            foreach (CharacterSaveData character in characterData)
            {
                character.LoadData(saveData);
            }

            foreach (CameraSaveData camera in cameraData)
            {
                camera.LoadData(saveData);
            }

            foreach (ItemSaveData item in itemData)
            {
                item.LoadData(saveData);
            }

            foreach (StorageSaveData storage in storageData)
            {
                storage.LoadData(saveData);
            }

            foreach (BooleanSaveData bools in booleanData)
            {
                bools.LoadData(saveData);
                var loadable = bools.GetComponent<IILoadable>();
                if (loadable != null) loadable.Load();
            }


        }

        public void SaveBoolean(SaveData saveData)
        {
            foreach (BooleanSaveData bools in booleanData)
            {
                bools.SaveData(saveData);
            }
        }
        public void CharacterLoader(SaveData saveData)
        {
            Character character = saveData.character;

            characterManager.characterName.characterName = character.characterName;
            characterManager.currentPlayer = character.PlayerChararter();
            characterManager.currentCamera = character.CharacterCamera();
            characterManager.currentAnimator = character.CharacterAnimator();
            characterManager.moveableCharacter = saveData.movableCharacter;
            Actions.SetCamera?.Invoke(character.CharacterCamera());
            Actions.CharacterSwap?.Invoke(character.PlayerChararter());
        }

        public void SaveCharacters(SaveData saveData)
        {
            foreach (CharacterSaveData character in characterData)
            {
                character.SaveData(saveData);
            }
        }
        public void SaveEnemies(SaveData saveData)
        {
            foreach (ZombieSaveData zombie in enemyZombie)
            {
                zombie.SaveData(saveData);
            }
        }
        public void SaveItems(SaveData saveData)
        {
            foreach (ItemSaveData item in itemData)
            {
                item.SaveData(saveData);
            }
        }
        public void SaveCameras(SaveData saveData)
        {
            foreach (CameraSaveData camera in cameraData)
            {
                
                camera.SaveData(saveData);
            }
        }
        public void SaveRoom(SaveData saveData)
        {
            foreach (RoomSaveData room in roomData)
            {       
                room.SaveData(saveData);
            }
        }
        public void SaveStorage(SaveData saveData)
        {
            foreach (StorageSaveData storage in storageData)
            {
                storage.SaveData(saveData);
            }
        }

        public void Increase()
        {
            firstAidSpray++;
            return;
        }
    }



}
