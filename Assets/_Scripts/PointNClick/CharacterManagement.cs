using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class CharacterSwapCalled : BaseMessage { public GameObject character; }

[System.Serializable]
public class CharacterManagement : SingletonBehavior<CharacterManagement>, IComponentSavable
{
    public Character characterName;
    [SerializeField] bool notSwappable;
    [SerializeField] PlayerInventory currentInventory;

    public Animator currentAnimator;
    
    public GameObject currentPlayer;
    public PlayerAction playerActions;

    public List<GameObject> characters = new List<GameObject>();

    public List<CharacterSpawnPoint> spawns;
    public List<CharacterData> characterData;
    private CharacterSwapCalled swappedCharacter = new CharacterSwapCalled();

    public GameObject Character
    {
        get { return currentPlayer; }
    }
    private IPlayerInput m_Input;
    public int currentCharacterIndex;

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
        
        playerActions = new PlayerAction();
        m_Input = GetComponent<IPlayerInput>();
        playerActions.Player.SwapCharacterUp.performed += ctx => SwapTrigger(1);
        playerActions.Player.SwapCharacterDown.performed += ctx => SwapTrigger(-1);

        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemyEventHandler.RegisterAllEnemiesInScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("SceneLoad");
        FindSpawns();
        SetCharacter(characters[currentCharacterIndex]);
    }

    public void FindSpawns()
    {
        CharacterSpawnPoint[] foundSpawns = UnityEngine.Object.FindObjectsOfType<CharacterSpawnPoint>(true);
        foreach (CharacterSpawnPoint newSpawn in foundSpawns)
        {
            if (!spawns.Contains(newSpawn))
            {
                spawns.Add(newSpawn);
            }
        }

        foreach (CharacterSpawnPoint spawnPoint in spawns)
        {
            if (!characterData.Contains(spawnPoint.characterToSpawn))
            {
                spawnPoint.RegisterPlayer(this.transform);
            }
        }

    }

    private void SwapTrigger(int i)
    {
        
        if (PauseController.Instance.IsPaused || characters.Count <= 1) return;
        SoundManagement.Instance.MenuSound(MenuSounds.Accept);
        SwapCharacter(i);

    }

    public void SwapCharacter(int i)
    {
        currentCharacterIndex += i;

        if (currentCharacterIndex >= characters.Count)
        {
            currentCharacterIndex = 0;
        }
        else if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characters.Count - 1;
        }

        SetCharacter(characters[currentCharacterIndex]);
    }

    private bool ShouldCharacterSwap(InputAction.CallbackContext context)
    {
        return context.performed &&
               !m_Input.IsAimingHeld() &&
               characters.Count > 1 &&
               !PauseController.Instance.IsPaused;
    }

    private void UpdateCharacterComponents()
    {
        currentPlayer.GetComponent<AudioSource>().volume = 1;
        EnableComponents(currentPlayer, true);

        foreach (var character in characters)
        {
            if (character != currentPlayer)
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

    private void SetCharacter(GameObject character)
    {
        currentPlayer = character;
        currentInventory = character.GetComponent<PlayerInventory>();
        currentAnimator = character.GetComponentInChildren<Animator>();

        swappedCharacter.character = currentPlayer;
        
        UpdateCharacterComponents();
        MessageBuffer<CharacterSwapCalled>.Dispatch(swappedCharacter);
    }

    #region InventoryManagement

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

    #endregion

    public void AddCharacter(GameObject character)
    {
        characters.Add(character);
    }

    public string GetSavableData()
    {
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
            _characters.Add(character);
        }
        return _characters;
    }

    public void SetFromSaveData(string savedData)
    {
        string[] entryDataList = savedData.Split('|');
        characters.Clear();

        foreach (string entryData in entryDataList)
        {
            GameObject character = GameObject.Find(entryData);

            if (character != null)
            {
                characters.Add(character);
            }
            else
            {
                Debug.LogError("Saved GameObject not found in scene: " + entryData);
            }
        }
    }
}
