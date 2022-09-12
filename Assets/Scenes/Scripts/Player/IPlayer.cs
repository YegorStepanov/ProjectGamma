using UnityEngine;

public interface IPlayer : IStateMachine<PlayerState>
{
    string Name { get; set; }
    uint Score { get; set; }
    Color32 Color { get; set; }

    PlayerData Data { get; }
    PlayerState State { get; }
    
    IInputManager InputManager { get; }

    void Move(Vector3 motion);
    void SetPosition(Vector3 position);

    Vector3 TransformDirection(Vector3 direction);
}
