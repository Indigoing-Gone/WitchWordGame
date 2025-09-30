using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Loom/Reciever")]
public class LoomReciever : ScriptableObject
{
    [SerializeField] private string triggerInput;
    [SerializeField] private GameState activeState;
    [SerializeField] private bool isActive;
    public event Action InputRecieved;

    private void OnEnable()
    {
        LoomController.InputCompleted += HandleInput;
        GameData.GameStateChanged += (_newState) => isActive = _newState == activeState;
    }

    private void OnDisable()
    {
        LoomController.InputCompleted -= HandleInput;
    }

    private void HandleInput(string _input)
    {
        if (!isActive) return;

        if (_input != triggerInput)
        {
            Span<char> _inputSpan = _input.ToCharArray();
            _inputSpan.Reverse();

            if (_inputSpan.ToString() != triggerInput) return;
        }

        InputRecieved?.Invoke();
    }
}
