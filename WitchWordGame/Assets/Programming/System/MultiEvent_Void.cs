using UnityEngine;

[CreateAssetMenu(menuName = "Events/MultiEvent Void")]
public class MultiEvent_Void : ScriptableObject
{
    [SerializeField] private GameEvent_Void[] events;
    private int eventsTriggered;

    [SerializeField] private GameEvent_Void multiEventTriggered;

    private void OnEnable()
    {
        for (int i = 0; i < events.Length; i++) events[i].AddListener(TrackEvents);
    }

    private void OnDisable()
    {
        for (int i = 0; i < events.Length; i++) events[i].RemoveListener(TrackEvents);
    }

    private void TrackEvents()
    {
        eventsTriggered++;
        if (eventsTriggered != events.Length) return;
        multiEventTriggered.TriggerEvent();
    }
}
