using UnityEngine;

namespace InputManagers
{
    public sealed class DefaultInputManager : IInputManager
    {
        public Vector3 ReadMoveVector()
        {
            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            input = Vector2.ClampMagnitude(input, 1f);
            return input;
        }

        public Vector2 ReadRotateVector()
        {
            Vector2 input;
            input.x = -Input.GetAxis("Mouse Y");
            input.y = Input.GetAxis("Mouse X");
            return input;
        }

        public bool ReadDashAction() =>
            Input.GetKeyDown(KeyCode.LeftControl);

        public bool ReadJumpAction() =>
            Input.GetKeyDown(KeyCode.Space);

        public bool ReadSprintAction() =>
            Input.GetKey(KeyCode.LeftShift);
    }
}
