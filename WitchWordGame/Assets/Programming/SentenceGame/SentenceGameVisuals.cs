using System;
using UnityEngine;
using UnityEngine.UI;

public class SentenceGameVisuals : MonoBehaviour
{
    [SerializeField] private RectTransform sentenceContainer;
    [SerializeField] private GameObject wordVisualPrefab;

    public void CreateSentenceVisuals(SentenceData _sentenceData)
    {
        for (int i = 0; i < _sentenceData.words.Length; i++)
        {
            GameObject _wordVisual = Instantiate(wordVisualPrefab, sentenceContainer);
            WordVisual _wordVisualScript = _wordVisual.GetComponent<WordVisual>();
            _wordVisualScript.AttachWord(_sentenceData.words[i]);
            _wordVisualScript.LayoutRebuilt += RebuildLayout;
        }

        RebuildLayout();
    }

    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer);
    }
}
