using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "SentenceGame/Sentence")]
public class SentenceData : ScriptableObject
{
    [TextArea(1, 4)]
    public string sentence;
    public Word[] words = new Word[0];
    public AudioClip spokenSentence;
    public int sentenceMana;

    public List<int> WordIndiciesWithData
    {
        get
        {
            List<int> _indices = new();
            for (int i = 0; i < words.Length; i++)
            {
                if (!words[i].hasClues || 
                    words[i].CluesUnlocked >= words[i].clues.Length || 
                    words[i].scramble != "") 
                    continue;
                _indices.Add(i);
            }
            return _indices;
        }
    }

    public void SentenceToWords()
    {
        if (string.IsNullOrWhiteSpace(sentence))
        {
            words = new Word[0];
            return;
        }

        var _matches = Regex.Matches(sentence, @"[\p{L}\p{N}'’_-]+|[^\s\p{L}\p{N}]");
        var _newWords = new Word[_matches.Count];

        for (int i = 0; i < _matches.Count; i++)
        {
            _newWords[i] = new Word {
                word = _matches[i].Value,
                hasClues = false,
                clues = new string[] { "Part of Speech", "Synonym" },
                CluesUnlocked = 0,
                scramble = ""
            };
        }

        words = _newWords;
    }

    public void IncrementCluesUnlocked(int _index, int _amount = 1)
    {
        if (_index >= words.Length) return;
        words[_index].CluesUnlocked += _amount;
    }

    public void RevealScramble(int _index)
    {
        if (_index >= words.Length || words[_index].scramble != "") return;
        words[_index].TriggerRevealScramble();
    }

    public void ResetData()
    {
        for (int i = 0; i < words.Length; i++)
        {
            if (!words[i].hasClues) continue;
            words[i].CluesUnlocked = 0;
            words[i].scramble = "";
        }

        sentenceMana = -1;
    }
}

[Serializable]
public class Word
{
    public string word;
    public bool hasClues;
    public string[] clues;
    public string scramble;

    [SerializeField] private int cluesUnlocked;
    public int CluesUnlocked
    {
        get => cluesUnlocked;
        set
        {
            if (!hasClues || value > clues.Length) return;
            cluesUnlocked = value;
            ClueUnlocked?.Invoke();
        }
    }

    public event Action ClueUnlocked;
    public event Action RevealScramble;

    public void TriggerRevealScramble()
    {
        List<char> _wordList = word.ToList();
        SentenceGameController.Shuffle(_wordList);
        scramble = new string(_wordList.ToArray());
        RevealScramble?.Invoke();
    }
}
