using UnityEngine;

public class MovementState : IPlayerState
{
    // Snap the character controller to the ground
    private const float DefaultVerticalSpeed = -2f;

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
        _verticalSpeed = DefaultVerticalSpeed;
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

        Vector3 horizontalVelocity = GetHorizontalVelocity();
        Vector3 verticalVelocity = _verticalSpeed * _player.Up;
        _player.Move(horizontalVelocity + verticalVelocity);
        UpdateMovementAnimation(horizontalVelocity);
    }

    private void UpdateIsGrounded()
    {
        Vector3 position = _player.Position;
        position += _player.Up * _player.Settings.GroundProbingOffset;
        _isGrounded = Physics.CheckSphere(position, _player.Settings.GroundProbingRadius, _player.Settings.GroundProbingLayers);
    }

    private void UpdateVerticalSpeed()
    {
        if (!_isGrounded)
            _verticalSpeed += _player.Settings.Gravity * Time.deltaTime;
        else
            _verticalSpeed = DefaultVerticalSpeed;

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

    private Vector3 GetHorizontalVelocity()
    {
        Vector3 moveInputVector = _player.InputManager.ReadMoveVector();
        Vector3 targetHorizontalVelocity = moveInputVector * GetPlayerSpeed();
        Vector3 currentHorizontalVelocity = Vector3.ProjectOnPlane(_player.Velocity, _player.Up);
        Vector3 horizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, _player.Settings.SpeedChangeRate * Time.deltaTime);
        return horizontalVelocity;
    }

    private float GetPlayerSpeed() =>
        _player.InputManager.ReadSprintAction() ? _player.Settings.SprintSpeed : _player.Settings.WalkSpeed;

    private void UpdateMovementAnimation(Vector3 horizontalVelocity)
    {
        float maxWalkSpeed = horizontalVelocity.magnitude / _player.Settings.WalkSpeed;
        _player.Animator.SetMovementAnimation(maxWalkSpeed);
    }

}