﻿using UnityEngine;

public sealed class DashState : IPlayerState
{
    private readonly IPlayer _player;
    private readonly PlayerStateFunctions _functions;

    private float _remainingDistance;

    public DashState(IPlayer player, PlayerStateFunctions functions)
    {
        _player = player;
        _functions = functions;
    }

    public void Enter()
    {
        _remainingDistance = _player.Settings.DashDistance;
    }

    public void Exit() { }

    public void Update()
    {
        if (_remainingDistance <= 0)
        {
            _player.StateMachine.SetState(PlayerState.Walk);
            return;
        }

        float speed = GetSpeed();
        speed = ClampSpeed(speed);

        _functions.Move(_player.Forward, speed, Vector3.zero);

        _remainingDistance -= speed * Time.deltaTime;
    }

    private float GetSpeed()
    {
        float percentage = _remainingDistance / _player.Settings.DashDistance;
        float speed = _player.Settings.DashSpeed.Evaluate(percentage) * _player.Settings.DashMaxSpeed;
        return speed;
    }

    private float ClampSpeed(float speed)
    {
        if (speed * Time.deltaTime > _remainingDistance)
            return _remainingDistance / Time.deltaTime;

        return speed;
    }
}