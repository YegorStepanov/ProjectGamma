using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkAnimator
{
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int FallBack = Animator.StringToHash("FallBack");

    // slow? Try to check performance and rework to events
    public bool IsMovementBlocked =>
        animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Getting Up");

    public void TriggerFallBackAnimation() =>
        SetTrigger(FallBack);

    public void TriggerKickAnimation() =>
        SetTrigger(Kick);

    public void SetMovementAnimation(float speed) =>
        animator.SetFloat(MovementSpeed, speed);

    public void SetIdleAnimation() =>
        SetMovementAnimation(0f);

    public void SetFallAnimation() =>
        animator.SetBool(Fall, true);

    public void UnsetFallAnimation() =>
        animator.SetBool(Fall, false);
}