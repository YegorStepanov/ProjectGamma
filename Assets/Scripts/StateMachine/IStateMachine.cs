// ReSharper disable once TypeParameterCanBeVariant
// contravariance is slow

public interface IStateMachine<TState> where TState : System.Enum
{
    TState State { get; }

    void SetState(TState state);
}
