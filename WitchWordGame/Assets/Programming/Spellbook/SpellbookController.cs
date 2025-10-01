using System;
using UnityEngine;
using UnityEngine.Windows;

public class SpellbookController : MonoBehaviour
{
    [SerializeField] private LoomReciever openSpellbook;
    [SerializeField] private LoomReciever left;
    [SerializeField] private LoomReciever right;
    [SerializeField] private LoomReciever closeSpellbook;

    [SerializeField] private GameObject[] pages;
    private int currentIndex = 0;

    [SerializeField] private GameObject book;
    [SerializeField] private GameObject bg;

    static public event Action SpellbookOpened;
    static public event Action SpellbookClosed;

    private void OnEnable()
    {
        left.InputRecieved += FlipLeft;
        right.InputRecieved += FlipRight;
        openSpellbook.InputRecieved += OpenBook;
        closeSpellbook.InputRecieved += CloseBook;
    }

    private void OnDisable()
    {
        left.InputRecieved -= FlipLeft;
        right.InputRecieved -= FlipRight;
        openSpellbook.InputRecieved -= OpenBook;
        closeSpellbook.InputRecieved -= CloseBook;
    }

    private void FlipLeft()
    {
        pages[currentIndex].SetActive(false);

        if(currentIndex == 0)
        {
            currentIndex = pages.Length - 1;
        }
        else
        {
            currentIndex--;
        }

        pages[currentIndex].SetActive(true);
    }

    private void FlipRight()
    {
        pages[currentIndex].SetActive(false);

        if (currentIndex == pages.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        pages[currentIndex].SetActive(true);
    }

    private void OpenBook()
    {
        bg.SetActive(true);
        book.SetActive(true);
        SpellbookOpened?.Invoke();
    }

    private void CloseBook()
    {
        bg.SetActive(false);
        book.SetActive(false);
        SpellbookClosed?.Invoke();
    }
}
