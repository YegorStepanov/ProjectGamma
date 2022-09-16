public sealed class NoneState : IPlayerState
{
    public static readonly NoneState Instance = new NoneState();

    public void Enter() { }

    public void Exit() { }

    public void Update() { }
}
