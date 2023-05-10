using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WalkState : IPlayerState
{
    private readonly Player _player;
    private readonly PlayerStateFunctions _functions;

    private Vector3 _gravityAcceleration = Vector3.zero;

    public WalkState(Player player, PlayerStateFunctions functions)
    {
        _player = player;
        _functions = functions;
    }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        if (_functions.IsDashing())
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
    }
}