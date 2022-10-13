using UnityEngine;

namespace InputManagers
{
    public interface IInputManager
    {
        public Vector3 ReadMoveAction();
        public Vector2 ReadRotateAction();
        public bool ReadDashAction();
    }
}
