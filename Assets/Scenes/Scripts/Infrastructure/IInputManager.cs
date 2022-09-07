using UnityEngine;

public interface IInputManager
{
    public Vector2 ReadMoveAction();
    public Vector2 ReadRotateAction();
    public bool ReadJumpAction();
}
