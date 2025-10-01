using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

public class SentenceGameController : MonoBehaviour
{
    [Header("Components")]
    private SentenceGameDisplay sentenceVisuals;
    private SentenceData sentenceData;
    [SerializeField] private LoomReciever exitInput;
    [SerializeField] private GameEvent_Audio speakSentenceEvent;

    [Header("Spell Parameters")]
    [SerializeField] private SentenceSpell[] spells;
    [SerializeField] private int maxMana = 3;

    public static event Action SentenceGameEntered;
    public static event Action SentenceGameExited;

    private void OnEnable()
    {
        MissionGiver.EnteringSentenceGame += OnEnteringSentenceGame;
        exitInput.InputRecieved += OnExitingSentenceGame;
    }

    private void OnDisable()
    {
        MissionGiver.EnteringSentenceGame -= OnEnteringSentenceGame;
        exitInput.InputRecieved -= OnExitingSentenceGame;
    }

    private void Awake()
    {
        sentenceVisuals = GetComponent<SentenceGameDisplay>();
    }

    private void OnEnteringSentenceGame(SentenceData _sentenceData)
    {
        sentenceData = _sentenceData;
        sentenceVisuals.CreateSentenceVisuals(sentenceData);

        if(sentenceData.sentenceMana == -1) sentenceData.sentenceMana = maxMana;

        for (int i = 0; i < spells.Length; i++) spells[i].CastingSpell += ProcessSpell;

        speakSentenceEvent.TriggerEvent(sentenceData.spokenSentence);
        SentenceGameEntered?.Invoke();
    }

    private void OnExitingSentenceGame()
    {
        for (int i = 0; i < spells.Length; i++) spells[i].CastingSpell -= ProcessSpell;
        sentenceVisuals.DeleteSentenceVisuals();

        SentenceGameExited?.Invoke();
    }

    private void ProcessSpell(SentenceSpell _spell)
    {
        if (_spell.manaCost > sentenceData.sentenceMana)
        {
            Debug.Log("Too little mana to cast");
            return;
        }

        sentenceData.sentenceMana -= _spell.manaCost;

        List<int> _selectedWordIndicies = SelectWords(_spell.numberOfWordsChecked, _spell.chanceOfSuccess);

        for (int i = 0; i < _selectedWordIndicies.Count; i++)
        {
            if (_spell.spellType == SpellType.AddClue)
                sentenceData.IncrementCluesUnlocked(_selectedWordIndicies[i]);
            if (_spell.spellType == SpellType.Scramble)
                sentenceData.RevealScramble(_selectedWordIndicies[i]);
        }
    }

    private List<int> SelectWords(float numberOfWordsChecked, float _chanceOfSuccess)
    {
        List<int> _optionIndicies = sentenceData.WordIndiciesWithData;
        Shuffle(_optionIndicies);
        int _optionsSelected = (int)Mathf.Ceil(numberOfWordsChecked * _optionIndicies.Count);

        List<int> _selectedIndicies = _optionIndicies.Take(_optionsSelected).ToList();
        List<int> _selectedWords = new();

        Random _rng = new();

        for (int i = 0; i < _selectedIndicies.Count; i++)
        {
            int _wordIndex = _selectedIndicies[i];

            double _diceRoll = _rng.NextDouble();
            if (_diceRoll > _chanceOfSuccess)
            {
                Debug.Log($"Fail on word {_wordIndex}");
                continue;
            }

            Debug.Log($"Success on word {_wordIndex}");

            _selectedWords.Add(_wordIndex);
        }

        return _selectedWords;
    }

    static public void Shuffle<T>(List<T> _list)
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
