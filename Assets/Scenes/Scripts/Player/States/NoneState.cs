public sealed class NoneState : IState
{
    public static NoneState Instance = new NoneState();

    public void Enter() { }

    public void Exit() { }
}
