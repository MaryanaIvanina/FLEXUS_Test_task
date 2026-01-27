using UnityEngine;

public interface IControllable
{
    Transform transform { get; }
    GameObject GameObject { get; }
    void SetControlActive(bool isActive);
}