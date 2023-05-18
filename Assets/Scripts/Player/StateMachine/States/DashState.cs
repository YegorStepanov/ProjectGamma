using UnityEngine;

public sealed class DashState : IPlayerState
{
    private const float ZeroPrecision = 0.01f;

    private readonly Player _player;
    private readonly PlayerStateFunctions _functions;

    private float _remainingDistance;

    public DashState(Player player, PlayerStateFunctions functions)
    {
        _player = player;
        _functions = functions;
    }

    public void Enter()
    {
        _remainingDistance = _player.Settings.DashDistance;
        _player.Animator.TriggerKickAnimation();
    }

    public void Exit() { }

    public void Update()
    {
        if (_remainingDistance <= ZeroPrecision)
        {
            MoveRemainingDistance();

            _player.StateMachine.SetState(PlayerState.Walk);
            return;
        }

        float speed = GetSpeed();
        speed = ClampSpeed(speed);

        Move(speed);
    }

    private void Move(float speed)
    {
        _functions.Move(_player.Forward, speed, Vector3.zero);
        _remainingDistance -= speed * Time.deltaTime;
    }

    private void MoveRemainingDistance()
    {
        _functions.Move(_player.Forward, _remainingDistance / Time.deltaTime, Vector3.zero);
        _remainingDistance = 0f;
    }

    private float GetSpeed()
    {
        float completedPercentage = 1f - _remainingDistance / _player.Settings.DashDistance;
        return ReadSpeedPercentageCurve(completedPercentage) * _player.Settings.DashMaxSpeed;
    }

    private float ReadSpeedPercentageCurve(float percentage)
    {
        float speedPercentage = _player.Settings.DashSpeedPercentage.Evaluate(percentage);
        // defense: if the value on the curve is zero, there will be an endless loop
        return speedPercentage <= ZeroPrecision ? ZeroPrecision : speedPercentage;
    }

    private float ClampSpeed(float speed)
    {
        if (speed * Time.deltaTime > _remainingDistance)
            return _remainingDistance / Time.deltaTime;

        return speed;
    }
}