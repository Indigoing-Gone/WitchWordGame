using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent Void")]
public class GameEvent_Void : ScriptableObject
{
    private List<GameEventListener_Void> eventListeners;

    public void TriggerEvent()
    {
        for (int i = 0; i < eventListeners.Count; i++) eventListeners[i].OnEventTriggered();
    }

    public void AddListener(GameEventListener_Void _listener) => eventListeners.Add(_listener);
    public void RemoveListener(GameEventListener_Void _listener) => eventListeners.Remove(_listener);
}
