using UnityEngine;

public sealed class DashState : IPlayerState
{
    private const float ZeroThreshold = 0.01f;

    private readonly Player _player;

    private float _remainingDistance;

    public DashState(Player player)
    {
        _player = player;
    }

    public void Enter()
    {
        _remainingDistance = _player.Settings.DashDistance;
        _player.Animator.TriggerKickAnimation();
    }

    public void Exit() { }

    public void Update()
    {
        if (_remainingDistance <= ZeroThreshold)
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
        _player.Move(_player.Forward * speed);
        _remainingDistance -= speed * Time.deltaTime;
    }

    private void MoveRemainingDistance()
    {
        float speed = _remainingDistance / Time.deltaTime;
        _player.Move(_player.Forward * speed);
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
        return speedPercentage <= ZeroThreshold ? ZeroThreshold : speedPercentage;
    }

    private float ClampSpeed(float speed)
    {
        if (speed * Time.deltaTime > _remainingDistance)
            return _remainingDistance / Time.deltaTime;

        return speed;
    }
}