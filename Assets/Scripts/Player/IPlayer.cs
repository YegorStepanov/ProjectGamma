using System;
using UnityEngine;

public interface IPlayer
{
    event Action<Player, ControllerColliderHit> Hit;
    event Action<Player> LocalPlayerStarted;
    event Action<Player> Destroying;

    IPlayerData Data { get; set; }
    IStateMachine<PlayerState> StateMachine { get; }
    Vector3 Position { get; set; }
    Quaternion Rotation { get; set; }
    bool IsGrounded { get; }

    Vector3 Up { get; }
    Vector3 Forward { get; }
    Vector3 Right { get; }

    PlayerSettings Settings { get; }
    IInputManager InputManager { get; }
    Transform CameraFocusPoint { get; }

    void Move(Vector3 motion);
}
