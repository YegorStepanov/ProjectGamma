using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkAnimator
{
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int IsNoneState = Animator.StringToHash("IsNoneState");

    // slow? Try to check performance and rework to events
    public bool IsMovementBlocked =>
        animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Getting Up");

    public void TriggerFallAnimation() =>
        SetTrigger(Fall);

    public void TriggerKickAnimation() =>
        SetTrigger(Kick);

    public void UpdateMovementAnimation(Vector3 moveDirection) =>
        animator.SetFloat(MovementSpeed, moveDirection.magnitude);

    public void SetIsNoneState(bool isSet) =>
        animator.SetBool(IsNoneState, isSet);
}