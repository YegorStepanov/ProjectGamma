using UnityEngine;

public class MovementState : IPlayerState
{
    private readonly Player _player;

    private float _verticalVelocity; //todo -> it should be velocity instead speed

    public MovementState(Player player)
    {
        _player = player;
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

        if (_player.InputManager.ReadDashAction())
        {
            _player.StateMachine.SetState(PlayerState.Dash);
            return;
        }

        //add terminalGravityVelocity
        if (!_player.IsGrounded)
            _verticalVelocity += _player.Settings.Gravity * Time.deltaTime; //todo
        else
            _verticalVelocity = 0f;


        if (_player.InputManager.ReadJumpAction())
        {
            _verticalVelocity = Mathf.Sqrt(_player.Settings.JumpHeight * -2f * _player.Settings.Gravity);

            // if (_jumpTimeoutDelta >= 0.0f)
            //     _jumpTimeoutDelta -= Time.deltaTime;
        }

        Vector3 moveInputVector = _player.InputManager.ReadMoveVector();
        Vector3 moveVelocity = moveInputVector * _player.Settings.WalkSpeed + _verticalVelocity * _player.Up;

        _player.Move(moveVelocity);
        _player.Animator.UpdateMovementAnimation(moveInputVector);
    }
}