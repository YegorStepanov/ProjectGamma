using UnityEngine;

namespace InputManagers
{
    public sealed class EmptyInputManager : IInputManager
    {
        public static readonly EmptyInputManager Instance = new EmptyInputManager();

        private EmptyInputManager() { }

        public Vector3 ReadMoveAction() => default;
        public Vector2 ReadRotateAction() => default;
        public bool ReadDashAction() => default;
    }
}
