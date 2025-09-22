using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

[CreateAssetMenu(menuName = "Input/InputReader")]
[DefaultExecutionOrder(-1)]
public class InputReader : ScriptableObject, IPlayerActions
{
    private PlayerInputActions playerInput;

    #region Callbacks
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputActions();
            playerInput.Player.SetCallbacks(this);
        }

        DisableAll();
        EnablePlayer();
    }

    public void DisableAll()
    {
        playerInput.Player.Disable();
    }

    public void EnablePlayer()
    {
        playerInput.Player.Enable();
    }
    #endregion

    #region Events
    //Player
    public event Action<Vector2> PositionEvent;
    public event Action<bool> ClickEvent;
    #endregion

    #region Triggers
    //Mouse Position
    public void OnPosition(InputAction.CallbackContext context)
    {
        PositionEvent?.Invoke(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()));
    }

    //Drag
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) ClickEvent?.Invoke(true);
        if (context.phase == InputActionPhase.Canceled) ClickEvent?.Invoke(false);
    }
    #endregion
}