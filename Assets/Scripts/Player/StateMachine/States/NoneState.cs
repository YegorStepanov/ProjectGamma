using UnityEngine;

public sealed class NoneState : IPlayerState
{
    private readonly Player _player;

    public NoneState(Player player) =>
        _player = player;

    public void Enter()
    {
        _player.Animator.SetMovementAnimation(Vector3.zero); // i forgot what is it??
        _player.Animator.SetIsNoneState(true); //todo
    }

    public void Exit()
    {
        _player.Animator.SetIsNoneState(false); //todo
    }

    public void Update() { }
}
