using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private HashSet<MissionData> modifiedMissionData;
    [SerializeField] private HashSet<SentenceData> modifiedSentenceData;

    [SerializeField] private GameEvent_Audio audioOneShot;

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

        MissionGiver.StartingMission += (_data) => modifiedMissionData.Add(_data);
        MissionGiver.EnteringSentenceGame += (_data) => modifiedSentenceData.Add(_data);

        modifiedMissionData = new();
        modifiedSentenceData = new();
        GameState = GameState.Navigation;
    }

    private void OnDisable()
    {
        foreach (MissionData _data in modifiedMissionData) _data.ResetData();
        foreach (SentenceData _data in modifiedSentenceData) _data.ResetData();
    }
}

public enum GameState
{
    None,
    Navigation,
    Dialogue,
    SentenceGame
}