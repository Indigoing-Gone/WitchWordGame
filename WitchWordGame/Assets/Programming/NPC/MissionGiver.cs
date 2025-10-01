using System;
using UnityEngine;

[RequireComponent(typeof(Speaker))]
public class MissionGiver : MonoBehaviour
{
    [Header("Data")]
    private Speaker speaker;
    [SerializeField] private MissionData missionData;
    [SerializeField] private SentenceData sentenceData;

    [Header("Mission Events")]
    [SerializeField] private GameEvent_Void startMissionEvent;
    [SerializeField] private GameEvent_Void endMissionEvent;

    [Header("Input")]
    [SerializeField] private LoomReciever enterConversationInput;

    static public event Action<SentenceData> EnteringSentenceGame;
    static public event Action<MissionData> StartingMission;

    private void OnEnable()
    {
        speaker.ConversationExited += OnConversationExited;
        enterConversationInput.InputRecieved += Speak;
    }

    private void OnDisable()
    {
        speaker.ConversationExited -= OnConversationExited;
        enterConversationInput.InputRecieved -= Speak;
    }

    private void Awake()
    {
        speaker = GetComponent<Speaker>();
    }

    public void Speak()
    {
        speaker.ChangeConversation(missionData.CurrentConversation);
        speaker.EnterConversation();
    }

    private void OnConversationExited()
    {
        if (missionData.CurrentConversation.triggerSentenceGame) EnteringSentenceGame?.Invoke(sentenceData);
        if (missionData.MissionStatus == MissionStatus.NotStarted)
        {
            StartingMission?.Invoke(missionData);
            startMissionEvent.TriggerEvent();
        }
        else if (missionData.MissionStatus == MissionStatus.Completed) endMissionEvent.TriggerEvent();
    }
}