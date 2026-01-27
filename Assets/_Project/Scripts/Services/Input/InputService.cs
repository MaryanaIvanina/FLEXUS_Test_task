using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputService : IInputService, IDisposable
{
    private readonly GameControl _control;

    public InputService()
    {
        _control = new GameControl();
        _control.Enable();
        _control.Player.Interact.performed += OnInteractPerformed;
    }

    public void Dispose()
    {
        _control.Player.Interact.performed -= OnInteractPerformed;
        _control.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    public Vector2 MoveInput => _control.Player.Move.ReadValue<Vector2>();
    public bool IsSprintPressed => _control.Player.Sprint.IsPressed();
    public bool IsBraking => _control.Player.Brake.IsPressed();

    public event Action OnInteract;
}
