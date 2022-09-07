using UnityEngine;

public sealed class InputManager : MonoBehaviour, IInputManager
{
    public Vector2 ReadMoveAction()
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
        // input.x = Input.GetAxis("Vertical Camera");//Input.GetAxis("Mouse X");
        // input.y = Input.GetAxis("Horizontal Camera");//Input.GetAxis("Mouse Y");
        input.x = -Input.GetAxis("Mouse Y");
        input.y = Input.GetAxis("Mouse X");
        return input;
    }

    public bool ReadJumpAction()
    {
        // Input.GetButtonDown("Jump");
        return Input.GetKeyDown(KeyCode.Space);
    }
}
