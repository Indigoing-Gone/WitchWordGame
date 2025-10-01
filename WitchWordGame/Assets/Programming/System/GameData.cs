using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private GameState gameState;
    private HashSet<MissionData> modifiedMissionData;
    private HashSet<SentenceData> modifiedSentenceData;

    [SerializeField] private GameEvent_Audio audioOneShot;
    [SerializeField] private GameEvent_Void[] sentenceRefreshEvents;

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

        SpellbookController.SpellbookOpened += () => GameState = GameState.Spellbook;
        SpellbookController.SpellbookClosed += () => GameState = GameState.Navigation;

        MissionGiver.StartingMission += (_data) => modifiedMissionData.Add(_data);
        MissionGiver.EnteringSentenceGame += (_data) => modifiedSentenceData.Add(_data);

        audioOneShot.AddListener(PlayAudio);
        for (int i = 0; i < sentenceRefreshEvents.Length; i++)
            sentenceRefreshEvents[i].AddListener(ResetSentenceData);


        audioSource = GetComponent<AudioSource>();

        modifiedMissionData = new();
        modifiedSentenceData = new();
        GameState = GameState.Navigation;
    }

    private void OnDisable()
    {
        ResetSentenceData();
        foreach (MissionData _data in modifiedMissionData) _data.ResetData();

        for (int i = 0; i < sentenceRefreshEvents.Length; i++)
            sentenceRefreshEvents[i].RemoveListener(ResetSentenceData);
    }

    private void PlayAudio(AudioClip _audioClip)
    {
        audioSource.clip = _audioClip;
        audioSource.Play();
    }

    private void ResetSentenceData()
    {
        foreach (SentenceData _data in modifiedSentenceData) _data.ResetData();
    }
}

public enum GameState
{
    None,
    Navigation,
    Dialogue,
    SentenceGame,
    Spellbook
}