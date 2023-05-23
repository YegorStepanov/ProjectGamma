using UnityEngine;

public class MovementState : IPlayerState
{
    private readonly Player _player;

    private float _verticalSpeed;
    private bool _previousIsGround;

    private bool _isGrounded;

    public MovementState(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _verticalSpeed = -2f;
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

        UpdateIsGrounded();
        UpdateVerticalSpeed();
        HandleJump();
        HandleFall();

        Vector3 moveInputVector = _player.InputManager.ReadMoveVector();
        Vector3 horizontalVelocity = moveInputVector * _player.Settings.WalkSpeed;
        Vector3 verticalVelocity = _verticalSpeed * _player.Up;
        _player.Move(horizontalVelocity + verticalVelocity);
        _player.Animator.SetMovementAnimation(moveInputVector);
    }

    private void UpdateIsGrounded()
    {
        Vector3 pos = _player.Position;
        pos += _player.Up * _player.Settings.GroundProbingOffset;
        _isGrounded = Physics.CheckSphere(pos, _player.Settings.GroundProbingRadius, _player.Settings.GroundProbingLayers);
    }

    private void UpdateVerticalSpeed()
    {
        if (!_isGrounded)
            _verticalSpeed += _player.Settings.Gravity * Time.deltaTime;
        else
            _verticalSpeed = 0f;

        _verticalSpeed = Mathf.Min(_verticalSpeed, _player.Settings.TerminalGravitySpeed);
    }

    private void HandleJump()
    {
        if (_isGrounded && _player.InputManager.ReadJumpAction())
        {
            _verticalSpeed = Mathf.Sqrt(-2f * _player.Settings.JumpHeight * _player.Settings.Gravity);
        }
    }

    private void HandleFall()
    {
        if (_isGrounded)
            _player.Animator.UnsetFallAnimation();
        else
            _player.Animator.SetFallAnimation();
    }
}