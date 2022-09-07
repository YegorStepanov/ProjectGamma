public interface IStateMachine
{
    public void SetState<T>() where T : IState;
}
