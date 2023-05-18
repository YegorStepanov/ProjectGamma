using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

    [SerializeField] private Animator _animator;

    public void TriggerFallAnimation() =>
        _animator.SetTrigger(Fall);

    public void TriggerKickAnimation() =>
        _animator.SetTrigger(Kick);

    public void UpdateMovementAnimation(Vector3 moveDirection) =>
        _animator.SetFloat(MovementSpeed, moveDirection.magnitude);
}