using System;
using UnityEngine;

public class GameEventListener_Void : MonoBehaviour
{
    [SerializeField] private GameEvent_Void gameEvent;
    public event Action EventTriggered;

    void OnEnable() => gameEvent.AddListener(this);
    void OnDisable() => gameEvent.RemoveListener(this);
    public void OnEventTriggered() => EventTriggered?.Invoke();
}
