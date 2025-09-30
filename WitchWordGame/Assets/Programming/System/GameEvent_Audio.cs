using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent Audio")]
public class GameEvent_Audio : ScriptableObject
{
    private event Action<AudioClip> GameEvent;

    public void AddListener(Action<AudioClip> method) => GameEvent += method;
    public void RemoveListener(Action<AudioClip> method) => GameEvent -= method;

    public void TriggerEvent(AudioClip audio) => GameEvent?.Invoke(audio);
}
