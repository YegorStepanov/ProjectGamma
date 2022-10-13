using UnityEngine;

namespace InputManagers
{
    public sealed class TransformRelativeInputManager : IInputManager
    {
        private readonly IInputManager _inputManager = new InputManager();
        private readonly Transform _relativeTo;

        public TransformRelativeInputManager(Transform relativeTo)
        {
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
