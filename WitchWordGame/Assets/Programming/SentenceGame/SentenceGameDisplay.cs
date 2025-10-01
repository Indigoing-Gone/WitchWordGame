using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceGameDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform sentenceContainer;
    [SerializeField] private GameObject wordVisualPrefab;
    [SerializeField] private GameObject sentenceGameBackground;
    [SerializeField] private GameObject sentenceGameUI;
    [SerializeField] private TextMeshProUGUI manaText;

    private void OnEnable()
    {
        sentenceGameBackground.SetActive(false);
        sentenceGameUI.SetActive(false);
    }

    public void CreateSentenceVisuals(SentenceData _sentenceData)
    {
        DeleteSentenceVisuals();

        for (int i = 0; i < _sentenceData.words.Length; i++)
        {
            GameObject _wordVisual = Instantiate(wordVisualPrefab, sentenceContainer);
            WordDisplay _wordVisualScript = _wordVisual.GetComponent<WordDisplay>();
            _wordVisualScript.AttachWord(_sentenceData.words[i]);
            _wordVisualScript.LayoutRebuilt += RebuildLayout;
        }

        sentenceGameBackground.SetActive(true);
        sentenceGameUI.SetActive(true);
        RebuildLayout();
    }

    public void DeleteSentenceVisuals()
    {
        foreach (Transform child in sentenceContainer.transform) Destroy(child.gameObject);
        sentenceGameBackground.SetActive(false);
        sentenceGameUI.SetActive(false);
    }

    public void ShowMana(int _mana)
    {
        manaText.text = _mana.ToString();
    }

    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer);
    }
}
