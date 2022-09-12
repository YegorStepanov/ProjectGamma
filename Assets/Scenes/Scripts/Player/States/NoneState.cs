public sealed class NoneState : IState
{
    public static readonly NoneState Instance = new NoneState();

    public void Enter() { }

    public void Exit() { }
}
