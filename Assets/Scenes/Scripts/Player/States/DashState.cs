using System;
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
        // _moveD = moveDirection;

        float speed = GetSpeed();
        speed = ConstrainSpeed(speed);

        bool onGround = TryGetGroundNormal(out Vector3 normal);
        if (onGround)
        {
            moveDirection= ProjectOnGround(moveDirection, normal);
            // _moveD2 = moveDirection;
            LookAt(moveDirection, normal);
            // moveDirection = transform.rotation * moveDirection;
        }

        Move(moveDirection, speed, Vector3.zero);

        _remainingDistance -= speed * Time.deltaTime;
    }

    // private Vector3 _moveD;
    // private Vector3 _moveD2;
    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color=Color.red;
    //     Gizmos.DrawRay(transform.position, _moveD);
    //     Gizmos.color=Color.green;
    //     Gizmos.DrawRay(transform.position, _moveD2);
    // }

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
