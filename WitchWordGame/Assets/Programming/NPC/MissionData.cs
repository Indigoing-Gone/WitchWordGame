using System;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Mission")]
public class MissionData : ScriptableObject
{
    [SerializeField] private MissionEvent[] missionEvents;
    [SerializeField] private int activeMissionEventId;

    public MissionStatus MissionStatus { get; private set; }
    public Conversation CurrentConversation { get => missionEvents[activeMissionEventId].conversation; }

    public event Action<MissionStatus> MissionUpdated;

    private void OnEnable()
    {
        ResetData();
    }

    private void OnDisable()
    {
        missionEvents[activeMissionEventId].progressEvent.RemoveListener(ProgressMission);
    }

    private void ProgressMission()
    {
        MissionStatus++;
        UpdateActiveMission();
    }

    private void UpdateActiveMission()
    {
        for (int i = 0; i < missionEvents.Length; i++)
        {
            if (missionEvents[i].missionStatus != MissionStatus) continue;

            missionEvents[activeMissionEventId].progressEvent.RemoveListener(ProgressMission);
            activeMissionEventId = i;

            if (activeMissionEventId >= missionEvents.Length ||
                missionEvents[activeMissionEventId].progressEvent == null) break;

            missionEvents[activeMissionEventId].progressEvent.AddListener(ProgressMission);
            break;
        }
    }

    public void ResetData()
    {
        MissionStatus = MissionStatus.NotStarted;
        activeMissionEventId = 0;
        UpdateActiveMission();
    }
}

[Serializable]
public enum MissionStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}

[Serializable]
public struct MissionEvent
{
    public MissionStatus missionStatus;
    public Conversation conversation;
    public GameEvent_Audio progressEvent;
}