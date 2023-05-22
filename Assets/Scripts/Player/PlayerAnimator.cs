using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkAnimator
{
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

    // slow? Try to check performance and rework to events
    public bool IsMovementBlocked =>
        animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Getting Up");

    public void TriggerFallAnimation() =>
        SetTrigger(Fall);

    public void TriggerKickAnimation() =>
        SetTrigger(Kick);

    public void SetMovementAnimation(Vector3 horizontalMoveDirection) =>
        animator.SetFloat(MovementSpeed, horizontalMoveDirection.magnitude);

    public void SetIdleAnimation() =>
        SetMovementAnimation(Vector3.zero);
}