using UnityEngine;

namespace InputManagers
{
    public sealed class RelativeInputManagerDecorator : IInputManager
    {
        private readonly IInputManager _inputManager;
        private readonly Transform _relativeTo;

        public RelativeInputManagerDecorator(IInputManager inputManager, Transform relativeTo)
        {
            _inputManager = inputManager;
            _relativeTo = relativeTo;
        }

        public Vector3 ReadMoveVector()
        {
            Vector3 input = _inputManager.ReadMoveVector();
            input = new Vector3(input.x, 0, input.y);
            input = _relativeTo.TransformDirection(input);
            return input;
        }

        public Vector2 ReadRotateVector() =>
            _inputManager.ReadRotateVector();

        public bool ReadDashAction() =>
            _inputManager.ReadDashAction();

        public bool ReadJumpAction() =>
            _inputManager.ReadJumpAction();

        public bool ReadSprintAction() =>
            _inputManager.ReadSprintAction();
    }
}
