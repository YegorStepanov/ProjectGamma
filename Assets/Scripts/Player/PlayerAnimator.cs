using Mirror;
using UnityEngine;

public class PlayerAnimator : NetworkAnimator
{
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

    public void TriggerFallAnimation() =>
        SetTrigger(Fall);

    public void TriggerKickAnimation() =>
        SetTrigger(Kick);

    public void UpdateMovementAnimation(Vector3 moveDirection) =>
        animator.SetFloat(MovementSpeed, moveDirection.magnitude);
}