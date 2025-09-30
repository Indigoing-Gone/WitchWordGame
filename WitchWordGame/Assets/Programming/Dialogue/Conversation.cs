using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Conversation", menuName = "NPC/Conversation")]
public class Conversation : ScriptableObject
{
    public Line[] lines;
    public Conversation nextConversation;
    public bool triggerSentenceGame;
}

[Serializable]
public struct Line
{
    public string name;
    [TextArea(1, 6)] public string text;
    public AudioClip bark;
}
