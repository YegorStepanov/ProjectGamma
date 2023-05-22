public sealed class NoneState : IPlayerState
{
    private readonly Player _player;

    public NoneState(Player player) =>
        _player = player;

    public void Enter()
    {
        _player.Animator.SetIdleAnimation();
        _player.Animator.SetIsNoneState(true);
    }

    public void Exit()
    {
        _player.Animator.SetIsNoneState(false);
    }

    public void Update() { }
}
