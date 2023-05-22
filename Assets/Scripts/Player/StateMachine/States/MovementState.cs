using UnityEngine;

public class MovementState : IPlayerState
{
    private readonly Player _player;

    private float _verticalSpeed;

    public MovementState(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _verticalSpeed = 0f;
    }

    public void Exit() { }

    public void Update()
    {
        if (_player.IsMovementBlocked)
            return;

        if (_player.InputManager.ReadDashAction())
        {
            _player.StateMachine.SetState(PlayerState.Dash);
            return;
        }

        if (!_player.IsGrounded)
            _verticalSpeed += _player.Settings.Gravity * Time.deltaTime;
        else
            _verticalSpeed = 0f;

        _verticalSpeed = Mathf.Min(_verticalSpeed, _player.Settings.TerminalGravitySpeed);

        if (_player.InputManager.ReadJumpAction())
        {
            _verticalSpeed = Mathf.Sqrt(_player.Settings.JumpHeight * -2f * _player.Settings.Gravity);

            // if (_jumpTimeoutDelta >= 0.0f)
            //     _jumpTimeoutDelta -= Time.deltaTime;
        }

        Vector3 moveInputVector = _player.InputManager.ReadMoveVector();
        Vector3 moveVelocity = moveInputVector * _player.Settings.WalkSpeed + _verticalSpeed * _player.Up;

        _player.Move(moveVelocity);
        _player.Animator.UpdateMovementAnimation(moveInputVector);
    }
}