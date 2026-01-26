using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool IsSprintPressed { get; }
    bool IsInteractPressed { get; }
    void Enable();
    void Disable();
}