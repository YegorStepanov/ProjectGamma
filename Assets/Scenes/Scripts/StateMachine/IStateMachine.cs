// ReSharper disable once TypeParameterCanBeVariant
// contravariance is slow
public interface IStateMachine<T> where T : System.Enum
{
    public void SetState(T state);
}
