using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

//public class CharacterSwapCalled : BaseMessage { public GameObject character; }

//[System.Serializable]
//public struct CharacterManagerSaveData
//{
//    public List<GameObject> Characters;
//}

[System.Serializable]
public class CharacterManager : SingletonBehavior<CharacterManager>, IComponentSavable
{

    //public static CharacterManager Instance { get; private set; }

    // Serialized Fields
    [SerializeField] Transform cameraLocation;
    [SerializeField] Transform characterManager;
    [SerializeField] bool notSwappable;
    [SerializeField] GameObject currentCharacter;
    [SerializeField] PlayerInventory currentInventory;


    // Public Properties
    public GameObject currentCamera;
    public Animator currentAnimator;
    public Character characterName;
    public GameObject currentPlayer;
    public PlayerAction playerActions;
    public List<GameObject> characters = new List<GameObject>();
    public List<CharacterData> characterData;
    public int moveableCharacter;

    public List<CharacterSpawnPoint> spawns;

    //private CharacterSwapCalled swappedCharacter = new CharacterSwapCalled();

    public GameObject Character
    {
        get { return currentPlayer; }
    }

    // Private Fields
    private IPlayerInput m_Input;

    #region Unity Lifecycle

    void OnEnable()
    {
        playerActions.Player.Enable();
    }

    void OnDisable()
    {
        playerActions.Player.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected override void Awake()
    {
            //FindSpawns();
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnemyEventHandler.RegisterAllEnemiesInScene();
            InitializeCharacterManager();
    }

    void Start()
    {
        SetupStartCharacter();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupStartCharacter();
    }

    public void FindSpawns()
    {
       CharactersCamera[] foundCameras = UnityEngine.Object.FindObjectsOfType<CharactersCamera>(true);
       CharacterSpawnPoint[] foundSpawns = UnityEngine.Object.FindObjectsOfType<CharacterSpawnPoint>(true);
       foreach(CharacterSpawnPoint newSpawn in foundSpawns)
        {
            if (!spawns.Contains(newSpawn))
            {
                spawns.Add(newSpawn);
            }
        }

       foreach(CharacterSpawnPoint spawnPoint in spawns)
        {
            if (!characterData.Contains(spawnPoint.characterToSpawn))
            {
                spawnPoint.RegisterPlayer(this.transform);
            }
        }

       foreach(Transform obj in this.transform)
        {
            if (!characters.Contains(obj.gameObject))
            {
                characters.Add(obj.gameObject);
            }
        }

        
        foreach (CharactersCamera camera in foundCameras)
        {
            foreach (GameObject character in characters)
            {
                var charCamera = character.GetComponent<EnableCharacterCamera>();
                if (camera.characterData == charCamera.character)
                {
                    charCamera.MyCamera = camera.gameObject;
                }
            }
        }

        

    }

    #endregion

    #region Initialization

    private void InitializeCharacterManager()
    {
        playerActions = new PlayerAction();
        m_Input = GetComponent<IPlayerInput>();
        playerActions.Player.SwapCharacterUp.performed += SwapTrigger;

        if (characters.Count > 0)
        {
            currentCharacter = characters[0];
        }

        if (currentPlayer != null)
        {

            Actions.CharacterSwap?.Invoke(currentPlayer);
            //swappedCharacter.character = currentPlayer;
            //MessageBuffer<CharacterSwapCalled>.Dispatch(swappedCharacter);
        }

        SwapMoveableCharacter();
    }

    private void SetupStartCharacter()
    {
        GetPlayer();
        Actions.CameraSwap?.Invoke();
        SwapMoveableCharacter();
    }

    #endregion

    #region Character Swap Logic

    private void SwapTrigger(InputAction.CallbackContext context)
    {
        if (ShouldCharacterSwap(context))
        {
            SoundManagement.Instance.MenuSound(MenuSounds.Accept);
            MoveableCharacterCycle();
            GetPlayer();
        }
    }

    private bool ShouldCharacterSwap(InputAction.CallbackContext context)
    {
        return context.performed &&
               !m_Input.IsAimingHeld() &&
               characters.Count > 1 &&
               Time.timeScale != 0f &&
               GameStateManager.Instance.CurrentGameState != GameState.Paused &&
               !Pause.paused;
    }

    private void MoveableCharacterCycle()
    {
        moveableCharacter = (moveableCharacter == 0) ? characters.Count - 1 : moveableCharacter - 1;

        SwapMoveableCharacter();

        Actions.CharacterSwap?.Invoke(currentPlayer);
        //swappedCharacter.character = currentPlayer;
        //MessageBuffer<CharacterSwapCalled>.Dispatch(swappedCharacter);
        Actions.CameraSwap?.Invoke();
    }

    private void SwapMoveableCharacter()
    {
        currentCharacter = characters[moveableCharacter];
        characterName.characterName = currentCharacter.GetComponent<PointAndClick>().character.character.characterName;

        UpdateCharacterComponents();
    }

    #endregion

    #region Component Management

    private void UpdateCharacterComponents()
    {
        currentCharacter.GetComponent<AudioSource>().volume = 1;
        EnableComponents(currentCharacter, true);

        foreach (var character in characters)
        {
            if (character != currentCharacter)
            {
                character.GetComponent<AudioSource>().volume = 0;
                EnableComponents(character, false);
            }
        }
    }

    private void EnableComponents(GameObject character, bool enabled)
    {
        var controllables = character.GetComponents<IControllable>();
        foreach (var controllable in controllables)
        {
            controllable.EnableControl(enabled);
        }

        ToggleNavMeshAgent(character, enabled);
    }

    private void ToggleNavMeshAgent(GameObject character, bool enabled)
    {
        var navAgent = character.GetComponent<NavMeshAgent>();
        if (navAgent && navAgent.enabled)
        {
            navAgent.isStopped = !enabled;
        }
    }

    #endregion

    #region Player Logic

    private void GetPlayer()
    {
        currentPlayer = null;

        foreach (Transform camera in cameraLocation)
        {
            var controller = camera.gameObject.GetComponent<CharactersCamera>();
            if (controller.playerContoller.enabled)
            {
                currentPlayer = controller.player;
                currentInventory = currentPlayer.GetComponent<PlayerInventory>();
                currentAnimator = currentPlayer.GetComponentInChildren<Animator>();
                currentCamera = controller.GetComponent<Camera>().gameObject;
                break;
            }
        }

        Actions.SetCamera?.Invoke(currentCamera);
        Actions.CharacterSwap?.Invoke(currentPlayer);
    }

    public void AddToCurrentInventory(Item item)
    {
        currentInventory.inventory.AddItem(item);
    }

    public bool CheckCurrentInventoryCount()
    {
        if (currentInventory.inventory.itemList.Count >= 8)
            return true;
        else
            return false;
    }

    public bool CheckKeyItem(Item.ItemType item)
    {
        foreach (Item listItem in currentInventory.inventory.itemList)
        {
            if (listItem.itemType == item)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveKeyItem(Item item)
    {
        currentInventory.inventory.RemoveItem(item);
    }

    public void RemoveKeyItem(Item.ItemType item)
    {
        currentInventory.inventory.RemoveItemType(item);
    }

    public void AddCharacter(GameObject character)
    {
        character.transform.SetParent(this.transform);
        characters.Add(character);
    }

    public void AddCharacter(CharacterData character)
    {
        //character.transform.SetParent(this.transform);
        characterData.Add(character);
    }

    #endregion

    public string GetSavableData()
    {
        print("Save");
        List<string> dataList = new List<string>();
        List<GameObject> characterList = Characters();

        for (int i = 0; i < characterList.Count; i++)
        {
            dataList.Add($"{characterList[i].ToString()}");
        }
        return string.Join("|", dataList);
    }

    public List<GameObject> Characters()
    {
        List<GameObject> _characters = new List<GameObject>();
        foreach (GameObject character in characters)
        {
            print(character);
            _characters.Add(character);
        }
        return _characters;
    }

    public void SetFromSaveData(string savedData)
    {
        print("LoadCharacters");
        string[] entryDataList = savedData.Split('|');
        characters.Clear();

        foreach (string entryData in entryDataList)
        {
            // Find the GameObject in the scene using the saved name
            GameObject character = GameObject.Find(entryData);

            if (character != null)
            {
                print(character);
                characters.Add(character);
            }
            else
            {
                // Handle the case where the GameObject is not found
                Debug.LogError("Saved GameObject not found in scene: " + entryData);
            }
        }
    }

}
