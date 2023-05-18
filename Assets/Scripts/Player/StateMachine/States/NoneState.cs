using UnityEngine;

public sealed class NoneState : IPlayerState
{
    private readonly Player _player;

    public NoneState(Player player) =>
        _player = player;

    public void Enter() =>
        _player.Animator.UpdateMovementAnimation(Vector3.zero);

    public void Exit() { }

    public void Update() { }
}
