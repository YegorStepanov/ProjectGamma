using UnityEngine;

public sealed class FakeInputManager : IInputManager
{
    public Vector2 ReadMoveAction() => Vector2.zero;
    public Vector2 ReadRotateAction() => Vector2.zero;
    public bool ReadJumpAction() => false;
}
