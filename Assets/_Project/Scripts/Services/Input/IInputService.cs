using UnityEngine;
using System;
using UnityEngine.InputSystem;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool IsSprintPressed { get; }
    bool IsBraking { get; }
    Vector2 LookInput { get; }

    event Action OnInteract;
}