using System;
using UnityEngine;

public class GameData : ScriptableObject
{
    [SerializeField] private GameState gameState;

    public GameState GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            GameStateChanged?.Invoke(gameState);
        }
    }

    static public event Action<GameState> GameStateChanged;

    public void ResetData()
    {
        GameState = GameState.Navigation;
    }
}

public enum GameState
{
    None,
    Navigation,
    Dialogue,
    SentenceGame
}