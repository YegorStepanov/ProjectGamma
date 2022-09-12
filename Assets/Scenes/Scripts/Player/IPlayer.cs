using UnityEngine;

public interface IPlayer : IStateMachine<PlayerState>
{
    PlayerData Data { get; }
    PlayerState State { get; }

    uint Score { get; }
    string Name { get; }

    IInputManager InputManager { get; }

    void Move(Vector3 motion);
    void SetPosition(Vector3 position);

    Vector3 TransformDirection(Vector3 direction);
}
