using System;
using UnityEngine;

public interface IPlayer
{
    event Action<Player, ControllerColliderHit> Hit;
    event Action<Player> LocalPlayerStarted;
    event Action<Player> InfoChanged;
    event Action<Player> Destroying;

    string Name { get; set; }
    uint Score { get; set; }
    Color32 Color { get; set; }

    PlayerData Data { get; }
    IInputManager InputManager { get; }
    PlayerState State { get; }
    Transform CameraFocusPoint { get; }

    void Move(Vector3 motion);
    void SetPosition(Vector3 position);
    void SetRotation(Quaternion rotation);
    void SetState(PlayerState none);
}
