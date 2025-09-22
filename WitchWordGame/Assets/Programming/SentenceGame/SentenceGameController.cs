using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

public class SentenceGameController : MonoBehaviour
{
    [Header("Sentence Data")]
    [SerializeField] private SentenceData sentenceData;

    [Header("Spell Parameters")]
    [SerializeField] private SentenceSpell[] spells;
    [SerializeField] private int maxMana = 3;
    [SerializeField] private int currentMana;

    private void OnEnable()
    {
        for (int i = 0; i < spells.Length; i++) spells[i].CastingSpell += ProcessSpell;
    }

    private void OnDisable()
    {
        for (int i = 0; i < spells.Length; i++) spells[i].CastingSpell -= ProcessSpell;
        sentenceData.ResetData();
    }

    private void Awake()
    {
        currentMana = maxMana;
    }

    private void ProcessSpell(SentenceSpell _spell)
    {
        if (_spell.manaCost > currentMana)
        {
            Debug.Log("Too little mana to cast");
            return;
        }

        currentMana -= _spell.manaCost;

        List<int> _selectedWordIndicies = SelectWords(_spell.numberOfWordsChecked);

        if (_spell.spellType == SpellType.AddClue)
        {
            AttemptAddClues(_selectedWordIndicies, _spell.chanceOfSuccess);
        }
    }

    private List<int> SelectWords(float numberOfWordsChecked)
    {
        List<int> _optionIndicies = sentenceData.WordIndiciesWithData;
        Shuffle(_optionIndicies);
        int _optionsSelected = (int)Mathf.Ceil(numberOfWordsChecked * _optionIndicies.Count);
        
        return _optionIndicies.Take(_optionsSelected).ToList();
    }

    private void AttemptAddClues(List<int> _selectedWordsIndicies, float _chanceOfSuccess)
    {
        Random _rng = new();
        
        for (int i = 0;  i < _selectedWordsIndicies.Count; i++)
        {
            int _wordIndex = _selectedWordsIndicies[i];

            double _diceRoll = _rng.NextDouble();
            if (_diceRoll > _chanceOfSuccess)
            {
                Debug.Log($"Fail on word {_wordIndex}");
                continue;
            }

            Debug.Log($"Success on word {_wordIndex}");

            sentenceData.IncrementCluesUnlocked(_wordIndex);
        }
    }

    private void Shuffle<T>(List<T> _list)
    {
        Random _rng = new();
        int n = _list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (_list[n], _list[k]) = (_list[k], _list[n]);
        }
    }
}
