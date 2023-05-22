using UnityEngine;

public class MovementState : IPlayerState
{
    private readonly Player _player;

    private float _verticalSpeed;
    private float _jumpTimeoutDelta;

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

        UpdateVerticalSpeed();
        HandleJump();

        Vector3 moveInputVector = _player.InputManager.ReadMoveVector();
        Vector3 moveVelocity = moveInputVector * _player.Settings.WalkSpeed + _verticalSpeed * _player.Up;

        _player.Move(moveVelocity);
        _player.Animator.SetMovementAnimation(moveInputVector);
    }

    private void UpdateVerticalSpeed()
    {
        if (!_player.IsGrounded)
            _verticalSpeed += _player.Settings.Gravity * Time.deltaTime;
        else
            _verticalSpeed = 0f;

        _verticalSpeed = Mathf.Min(_verticalSpeed, _player.Settings.TerminalGravitySpeed);
    }

    private void HandleJump()
    {
        if (_jumpTimeoutDelta > 0 && _player.InputManager.ReadJumpAction())
        {
            _verticalSpeed = Mathf.Sqrt(_player.Settings.JumpHeight * -2f * _player.Settings.Gravity);

            _jumpTimeoutDelta = _player.Settings.JumpTimeout;
        }
        else
        {
            if (_jumpTimeoutDelta >= 0.0f)
                _jumpTimeoutDelta -= Time.deltaTime;
        }
    }
}