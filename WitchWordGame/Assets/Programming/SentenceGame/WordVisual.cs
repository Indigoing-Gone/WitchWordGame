using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordVisual : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private TextMeshProUGUI[] cluesText;
    private Word word;

    public event Action LayoutRebuilt;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void AttachWord(Word _word)
    {
        word = _word;

        //Assign text
        wordText.text = word.word;
        if (word.hasClues) for (int i = 0; i < cluesText.Length && i < word.clues.Length; i++) cluesText[i].text = word.clues[i];
        
        //Disable Clues
        for (int i = 0; i < cluesText.Length; i++) cluesText[i].gameObject.SetActive(false);

        word.ClueUnlocked += OnClueUnlocked;

        RebuildLayout();
    }

    private void OnDisable()
    {
        word.ClueUnlocked -= OnClueUnlocked;
    }

    private void OnClueUnlocked()
    {
        int _clueIndexUnlocked = word.CluesUnlocked - 1;
        cluesText[_clueIndexUnlocked].gameObject.SetActive(true);
        RebuildLayout();
    }

    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        LayoutRebuilt?.Invoke();
    }
}
