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

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        if (_functions.IsDashPressed())
        {
            _player.StateMachine.SetState(PlayerState.Dash);
            return;
        }

        if (_player.IsGrounded)
            _gravityAcceleration = Vector3.zero;
        else
            _gravityAcceleration += Physics.gravity * Time.deltaTime;

        Vector3 moveDirection = _functions.GetMovementDirection();

        _functions.Move(moveDirection, _player.Settings.WalkSpeed, _gravityAcceleration);
        _player.Animator.UpdateMovementAnimation(moveDirection);
    }
}