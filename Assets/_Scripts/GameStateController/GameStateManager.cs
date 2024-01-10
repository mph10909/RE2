using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameStateManager
{
    private static GameStateManager _instance;

    public static GameStateManager Instance
    {
        get
        {
            if(_instance == null)
           
                _instance = new GameStateManager();

            return _instance;
        }
    }

    public GameState CurrentGameState { get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    private GameStateManager()
        {
        
        }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;
        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

    public void Pause()
    {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay
            ? GameState.Paused
            : GameState.GamePlay;

        GameStateManager.Instance.SetState(newGameState);

        Debug.Log(GameStateManager.Instance.CurrentGameState);
    }

}
