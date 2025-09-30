using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent Void")]
public class GameEvent_Void : ScriptableObject
{
    private event Action GameEvent;

    public void AddListener(Action method) => GameEvent += method;
    public void RemoveListener(Action method) => GameEvent -= method;

    public void TriggerEvent() => GameEvent?.Invoke();
}
