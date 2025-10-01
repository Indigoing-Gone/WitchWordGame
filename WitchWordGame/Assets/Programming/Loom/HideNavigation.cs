using System;
using UnityEngine;

public class HideNavigation : MonoBehaviour
{
    [SerializeField] private GameObject display;

    private void OnEnable()
    {
        SentenceGameController.SentenceGameEntered += () => DisplayNavigation(false);
        SentenceGameController.SentenceGameExited += () => DisplayNavigation(true);

        SpellbookController.SpellbookOpened += () => DisplayNavigation(false);
        SpellbookController.SpellbookClosed += () => DisplayNavigation(true);
    }

    private void DisplayNavigation(bool _state)
    {
        display.SetActive(_state);
    }
}
