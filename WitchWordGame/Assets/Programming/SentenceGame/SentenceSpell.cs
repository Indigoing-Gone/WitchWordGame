using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SentenceGame/Spell")]
public class SentenceSpell : ScriptableObject
{
    [Header("Loom Input")]
    public LoomReciever input;

    [Header("Spell Parameters")]
    public string spellName;
    public float chanceOfSuccess;
    public float numberOfWordsChecked;
    public SpellType spellType;
    public int manaCost;

    public event Action<SentenceSpell> CastingSpell;

    private void OnEnable()
    {
        input.InputRecieved += CastSpell;
    }

    private void OnDisable()
    {
        input.InputRecieved -= CastSpell;
    }

    private void CastSpell()
    {
        CastingSpell?.Invoke(this);
    }
}

public enum SpellType
{
    None,
    AddClue,
    Scramble
}