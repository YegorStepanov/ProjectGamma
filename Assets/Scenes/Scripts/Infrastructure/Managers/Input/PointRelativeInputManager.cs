using UnityEngine;

public sealed class PointRelativeInputManager : IInputManager
{
    private readonly IInputManager _inputManager = new InputManager();
    private readonly Transform _relativePoint;

    public PointRelativeInputManager(Transform relativePoint)
    {
        _relativePoint = relativePoint;
    }

    public Vector3 ReadMoveAction()
    {
        Vector3 input = _inputManager.ReadMoveAction();
        input = new Vector3(input.x, 0, input.y);
        input = _relativePoint.TransformDirection(input);
        return input;
    }

    public Vector2 ReadRotateAction()
    {
        return _inputManager.ReadRotateAction();
    }

    public bool ReadDashAction()
    {
        return _inputManager.ReadDashAction();
    }
}
