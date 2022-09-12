using UnityEngine;

public sealed class DashState : BasePlayerState
{
    private float _remainingDistance;

    public override void Enter()
    {
        _remainingDistance = Player.Data.DashDistance;
        base.Enter();
    }

    private void Update()
    {
        if (_remainingDistance <= 0)
        {
            StateMachine.SetState(PlayerState.Walk);
            return;
        }

        Vector3 moveDirection = transform.forward;

        float speed = GetSpeed();
        speed = ConstrainSpeed(speed);

        Vector3 normal = GetGroundNormal();
        moveDirection = ProjectOnGround(moveDirection, normal);

        Move(moveDirection, speed);
        LookAt(moveDirection, normal);

        _remainingDistance -= speed * Time.deltaTime;
    }

    private float GetSpeed()
    {
        float percentage = _remainingDistance / Player.Data.DashDistance;
        float speed = Player.Data.DashSpeed.Evaluate(percentage) * Player.Data.DashMaxSpeed;
        return speed;
    }

    private float ConstrainSpeed(float speed)
    {
        if (speed * Time.deltaTime > _remainingDistance)
            return _remainingDistance / Time.deltaTime;

        return speed;
    }
}
