using UnityEngine;

public sealed class CommonStateFunctions
{
    private const float RaycastDistance = 1.5f;

    private readonly Player _player;

    private int ExcludePlayerMask => ~(1 << _player.Data.Layer);

    public CommonStateFunctions(Player player)
    {
        _player = player.NotNull();
    }

    public bool IsDashPressed()
    {
        return _player.InputManager.ReadDashAction();
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 moveDirection = _player.InputManager.ReadMoveAction();

        if (IsMovementZero(moveDirection))
            return Vector3.zero;

        return moveDirection;
    }

    public void Move(Vector3 moveDirection, float speed, Vector3 gravityVelocity)
    {
        if (!IsMovementZero(moveDirection))
        {
            moveDirection.Normalize();

            if (TryGetGroundNormal(out Vector3 normal))
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, normal);
            }

        RotateVertically(normal);
        RotateHorizontally(moveDirection);

        Vector3 motion = (moveDirection * speed + gravityVelocity) * Time.deltaTime;
        _player.Move(motion);
    }

    private bool IsMovementZero(Vector3 moveDirection)
    {
        //settings null!?
        return moveDirection.sqrMagnitude < _player.Settings.MinMoveDistance * _player.Settings.MinMoveDistance;
    }

    private void RotateVertically(Vector3 upwards)
    {
        float step = _player.Settings.VerticalRotationSpeedRadians * Time.deltaTime;
        Vector3 up = Vector3.RotateTowards(_player.Up, upwards, step, 1f);

        Quaternion rotation = Quaternion.FromToRotation(_player.Up, up);
        _player.Rotation = rotation * _player.Rotation;
    }

    private void RotateHorizontally(Vector3 direction)
    {
        direction.Normalize();

        Vector3 directionXZ = Vector3.ProjectOnPlane(direction, _player.Up);

        Quaternion lookRotation = Quaternion.LookRotation(directionXZ, _player.Up);

        float step = _player.Settings.HorizontalRotationSpeedRadians * Time.deltaTime;
        _player.Rotation = Quaternion.RotateTowards(_player.Rotation, lookRotation, step * Mathf.Rad2Deg);
    }

    private bool TryGetGroundNormal(out Vector3 normal)
    {
        if (Physics.Raycast(_player.Position, -_player.Up, out RaycastHit hit, RaycastDistance, ExcludePlayerMask))
        {
            normal = hit.normal;
            return true;
        }

        normal = Vector3.zero;
        return false;
    }
}