using System;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState == value) return;
            gameState = value;
            GameStateChanged?.Invoke(gameState);
        }
    }

    static public event Action<GameState> GameStateChanged;

    private void OnEnable()
    {
        DialogueController.ConversationStarted += () => GameState = GameState.Dialogue;
        DialogueController.ConversationEnded += () => GameState = GameState.Navigation;

        SentenceGameController.SentenceGameEntered += () => GameState = GameState.SentenceGame;
        SentenceGameController.SentenceGameExited += () => GameState = GameState.Navigation;

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