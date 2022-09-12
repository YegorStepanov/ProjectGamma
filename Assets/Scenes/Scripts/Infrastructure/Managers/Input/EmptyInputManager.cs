using UnityEngine;

public sealed class EmptyInputManager : IInputManager
{
    public Vector2 ReadMoveAction() => default;
    public Vector2 ReadRotateAction() => default;
    public bool ReadDashAction() => default;
}
