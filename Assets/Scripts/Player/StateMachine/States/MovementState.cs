using UnityEngine;

public class MovementState : IPlayerState
{
    private readonly Player _player;
    private readonly CommonStateFunctions _functions;

    private float _verticalVelocity;

    public MovementState(Player player, CommonStateFunctions functions)
    {
        _player = player;
        _functions = functions;
    }

    public void Enter()
    {
        _verticalVelocity = 0f;
    }

    public void Exit() { }

    public void Update()
    {
        if (_player.IsMovementBlocked)
            return;

        if (_functions.IsDashPressed())
        {
            _player.StateMachine.SetState(PlayerState.Dash);
            return;
        }

        //add terminalGravityVelocity
        if (_functions.IsJumpPressed())
        {
            _verticalVelocity = Mathf.Sqrt(_player.Settings.JumpHeight * -2f * _player.Settings.Gravity);

            // if (_jumpTimeoutDelta >= 0.0f)
            //     _jumpTimeoutDelta -= Time.deltaTime;
        }

        if (!_player.IsGrounded)
            _verticalVelocity += _player.Settings.Gravity * Time.deltaTime; //todo

        Vector3 moveDirection = _functions.GetMovementDirection();

        _functions.Move(moveDirection, _player.Settings.WalkSpeed, _verticalVelocity);
        _player.Animator.UpdateMovementAnimation(moveDirection);
    }
}