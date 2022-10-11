// ReSharper disable once TypeParameterCanBeVariant
// reason: contravariance is slow

public interface IStateMachine<TState> where TState : System.Enum
{
    TState State { get; }

    void SetState(TState state);
}