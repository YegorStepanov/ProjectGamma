using UnityEngine;

namespace InputManagers
{
    public sealed class InputManager : IInputManager
    {
        public Vector3 ReadMoveAction()
        {
            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            input = Vector2.ClampMagnitude(input, 1f);
            return input;
        }

        public Vector2 ReadRotateAction()
        {
            Vector2 input;
            input.x = -Input.GetAxis("Mouse Y");
            input.y = Input.GetAxis("Mouse X");
            return input;
        }

        public bool ReadDashAction()
        {
            return Input.GetKeyDown(KeyCode.LeftControl);
        }
    }
}
