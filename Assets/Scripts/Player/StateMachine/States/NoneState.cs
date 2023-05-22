public sealed class NoneState : IPlayerState
{
    private readonly Player _player;

    public NoneState(Player player) =>
        _player = player;

    public void Enter()
    {
        _player.Animator.SetIdleAnimation();
    }

    public void Exit() { }

    public void Update() { }
}
