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

        public Vector3 ReadMoveAction()
        {
            Vector3 input = _inputManager.ReadMoveAction();
            input = new Vector3(input.x, 0, input.y);
            input = _relativeTo.TransformDirection(input);
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
}
