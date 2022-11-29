using System;
using InputManagers;
using UnityEngine;

public interface IPlayer
{
    event Action<Player> Destroying;

    PlayerData Data { get; set; }
    PlayerStateMachine StateMachine { get; }
    Vector3 Position { get; set; }
    Quaternion Rotation { get; set; }
    bool IsGrounded { get; }
    Vector3 Velocity { get; }

    Vector3 Up { get; }
    Vector3 Forward { get; }
    Vector3 Right { get; }

    PlayerSettings Settings { get; }
    IInputManager InputManager { get; }
    Transform CameraFocusPoint { get; }
    PlayerCollider Collider { get; }

    void Construct(PlayerSettings settings, IInputManager inputManager);
    void Move(Vector3 motion);
}