using UnityEngine;

namespace InputManagers
{
    public sealed class EmptyInputManager : IInputManager
    {
        public static readonly EmptyInputManager Instance = new EmptyInputManager();

        private EmptyInputManager() { }

        public Vector3 ReadMoveVector() => default;
        public Vector2 ReadRotateVector() => default;
        public bool ReadDashAction() => default;
        public bool ReadJumpAction() => default;
    }
}
