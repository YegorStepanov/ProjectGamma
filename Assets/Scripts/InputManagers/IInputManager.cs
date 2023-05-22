using UnityEngine;

namespace InputManagers
{
    public interface IInputManager
    {
        public Vector3 ReadMoveVector();
        public Vector2 ReadRotateVector();
        public bool ReadDashAction();
        public bool ReadJumpAction();
    }
}
