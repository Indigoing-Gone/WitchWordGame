using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "SentenceGame/Sentence")]
public class SentenceData : ScriptableObject
{
    [TextArea(1, 4)]
    public string sentence;
    public Word[] words = new Word[0];
    public List<int> WordIndiciesWithData
    {
        get
        {
            List<int> _indices = new();
            for (int i = 0; i < words.Length; i++)
            {
                if (!words[i].hasClues || words[i].cluesUnlocked > words[i].clues.Length) continue;
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
                cluesUnlocked = 0
            };
        }

        words = _newWords;
    }

    public void IncrementCluesUnlocked(int _index, int _amount = 1)
    {
        if (_index >= words.Length || !words[_index].hasClues) return;
        words[_index].cluesUnlocked += _amount;
    }

    public void ResetData()
    {
        for (int i = 0; i < words.Length; i++)
        {
            if (!words[i].hasClues) continue;
            words[i].cluesUnlocked = 0;
        }
    }
}

[Serializable]
public struct Word
{
    public string word;
    public bool hasClues;
    public string[] clues;
    public int cluesUnlocked;
}
