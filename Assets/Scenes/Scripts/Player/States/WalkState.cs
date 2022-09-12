using UnityEngine;

public sealed class WalkState : BasePlayerState
{
    private void Update()
    {
        if (IsDashing())
        {
            Player.SetState(PlayerState.Dash);
            return;
        }

        Vector3 moveDirection = GetMovementDirection();
        Vector3 normal = GetGroundNormal();
        moveDirection = ProjectOnGround(moveDirection, normal);

        Move(moveDirection, Player.Data.WalkSpeed);
        LookAt(moveDirection, normal);
    }
}
