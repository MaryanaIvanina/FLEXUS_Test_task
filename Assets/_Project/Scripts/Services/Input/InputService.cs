using UnityEngine;

public class InputService : IInputService
{
    private readonly GameControl _controls;

    public InputService()
    {
        _controls = new GameControl();
    }

    public Vector2 MoveInput => _controls.Player.Move.ReadValue<Vector2>();
    public bool IsSprintPressed => _controls.Player.Sprint.IsPressed();
    public bool IsInteractPressed => _controls.Player.Interact.WasPressedThisFrame();

    public void Enable() => _controls.Enable();
    public void Disable() => _controls.Disable();
}
