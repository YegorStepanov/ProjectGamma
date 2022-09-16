using UnityEngine;

public sealed class PlayerStateFunctions
{
    private readonly IPlayer _player;

    private int ExcludePlayerMask => ~(1 << _player.Data.Layer);

    public PlayerStateFunctions(IPlayer player)
    {
        _player = player;
    }

    public bool IsDashing()
    {
        return _player.InputManager.ReadDashAction();
    }

    public void Move(Vector3 moveDirection, float speed, Vector3 gravityVelocity)
    {
        moveDirection.Normalize();

        bool onGround = TryGetGroundNormal(out Vector3 normal);
        if (onGround)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, normal);
            LookAt(moveDirection, normal);
        }
        else
            LookAt(moveDirection, _player.Up);

        Move(moveDirection * speed, gravityVelocity);
    }

    private void Move(Vector3 moveVelocity, Vector3 gravityVelocity)
    {
        Vector3 motion = (moveVelocity + gravityVelocity) * Time.deltaTime;
        _player.Move(motion);
    }

    private void LookAt(Vector3 direction, Vector3 upwards)
    {
        if (direction.magnitude < _player.Settings.MinMoveDistance)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized, upwards);
        _player.Rotation = Quaternion.Slerp(_player.Rotation, lookRotation, _player.Settings.RotationSpeed * Time.deltaTime);
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 moveDirection = _player.InputManager.ReadMoveAction();

        if (moveDirection.magnitude < _player.Settings.MinMoveDistance)
            return Vector3.zero;

        return moveDirection;
    }

    private bool TryGetGroundNormal(out Vector3 normal)
    {
        if (Physics.Raycast(_player.Position, -_player.Up, out RaycastHit hit, 1.5f, ExcludePlayerMask))
        {
            normal = hit.normal;
            return true;
        }

        normal = Vector3.zero;
        return false;
    }
}
