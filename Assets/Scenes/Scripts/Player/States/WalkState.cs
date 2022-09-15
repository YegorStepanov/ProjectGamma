using System;
using UnityEngine;

public sealed class WalkState : BasePlayerState
{
    private CharacterController _controller;

    private Vector3 _gravityAcceleration = Vector3.zero;

    protected override void Awake()
    {
        _controller = GetComponent<CharacterController>();
        base.Awake();
    }

    private void Update()
    {
        if (IsDashing())
        {
            StateMachine.SetState(PlayerState.Dash);
            return;
        }

        if (_controller.isGrounded)
            _gravityAcceleration = Vector3.zero;
        else
            _gravityAcceleration += Physics.gravity * Time.deltaTime;

        Vector3 moveDirection = GetMovementDirection();

        if (moveDirection == Vector3.zero)
            return;

        // if (moveDirection.magnitude <= Player.Data.MinMoveDistance)
        // {
        //     //Move(Vector3.zero, 1f, Vector3.zero);
        //     return;
        // }

        _moveD = moveDirection;

        bool onGround = TryGetGroundNormal(out Vector3 normal);
        if (onGround)
        {
            _moveD2 = ProjectOnGround(moveDirection, normal);
            
            Debug.Log($"| {_moveD2.ToString("f4")} {_moveD2.magnitude}");
            // Debug.Log($"| {_moveD2.ToString("f4")} {_moveD2.magnitude} {moveDirection.ToString("f4")} {moveDirection.magnitude}");
            
            moveDirection = _moveD2;
            // LookAt(moveDirection, normal);
            // moveDirection = transform.rotation * moveDirection;
            LookAt(moveDirection, normal);

            if (name == "GamePlayer(Clone)")
            {
                // transform.rotation * Vector3.right;
                // == 
                // transform.TransformDirection(Vector3.right);
                // Debug.Log($"{transform.rotation.eulerAngles} {moveDirection} {moveDirection.magnitude};{normal};{moveDirection.magnitude} {moveDirection.x:00.000} {moveDirection.y:00.000} {moveDirection.z:00.000}");
            }
        }

        Move(moveDirection, Player.Data.WalkSpeed, _gravityAcceleration);
    }

    private Vector3 _moveD;
    private Vector3 _moveD2;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // Gizmos.DrawRay(transform.position, _moveD);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
