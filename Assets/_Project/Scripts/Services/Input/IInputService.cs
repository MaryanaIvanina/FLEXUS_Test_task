using UnityEngine;
using System;
using UnityEngine.InputSystem;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool IsSprintPressed { get; }
    bool IsBraking { get; }

    event Action OnInteract;
}