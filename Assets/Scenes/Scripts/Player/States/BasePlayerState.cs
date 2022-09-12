using Mirror;
using UnityEngine;

public abstract class BasePlayerState : NetworkBehaviour, IState
{
    protected IPlayer Player { get; private set; }

    protected virtual void Awake()
    {
        Player = GetComponent<IPlayer>();
        Exit();
    }

    public virtual void Enter() =>
        enabled = true;

    public virtual void Exit() =>
        enabled = false;

    protected void Move(Vector3 direction, float speed)
    {
        Vector3 motion = (direction * speed + Physics.gravity) * Time.deltaTime;
        Player.Move(motion);
    }

    protected void LookAt(Vector3 direction, Vector3 upwards)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction, upwards);
        lookRotation.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Player.Data.RotationSpeed * Time.deltaTime);
    }

    protected bool IsDashing()
    {
        return Player.InputManager.ReadDashAction();
    }

    protected Vector3 GetMovementDirection()
    {
        Vector3 input = ReadMovementInput();
        Vector3 direction = Player.TransformDirection(input);
        return direction;
    }

    protected Vector3 GetGroundNormal()
    {
        Vector3 heroDown = transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.position, heroDown, out RaycastHit hit))
            return hit.normal;

        return Vector3.zero;
    }

    protected static Vector3 ProjectOnGround(Vector3 direction, Vector3 normal)
    {
        if (normal == Vector3.zero)
            return direction;

        return Vector3.ProjectOnPlane(direction, normal); //should we normalize it?
    }

    private Vector3 ReadMovementInput()
    {
        Vector2 input = Player.InputManager.ReadMoveAction();
        return new Vector3(input.x, 0, input.y);
    }
}
