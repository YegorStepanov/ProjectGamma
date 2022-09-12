using UnityEngine;

public interface IPlayer
{
    bool IsLocalPlayer { get; }

    Transform RelativeMovementTo { get; }

    string Name { get; set; }
    uint Score { get; set; }
    Color32 Color { get; set; }

    PlayerData Data { get; }
    PlayerState State { get; }

    IInputManager InputManager { get; }

    void Move(Vector3 motion);
    void SetPosition(Vector3 position);
    void SetState(PlayerState none);
}
