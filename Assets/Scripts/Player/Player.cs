using System;
using InputManagers;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerAnimator))]
public sealed class Player : NetworkBehaviour, IPlayer
{
    public event Action<Player> Destroying;

    public PlayerData Data { get; set; }
    public PlayerStateMachine StateMachine { get; private set; }

    [SerializeField] private Transform _cameraFocusPoint;
    [SerializeField] private Transform _pivot;
    [SerializeField] private PlayerCollider _playerCollider;

    private CharacterController _controller;

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            // CharacterController should be disabled when we want change position directly
            bool isEnabled = _controller.enabled;
            if (isEnabled)
                _controller.enabled = false;

            transform.position = value;

            if (isEnabled)
                _controller.enabled = true;
        }
    }

    public Quaternion Rotation
    {
        get => _pivot.rotation;
        set => _pivot.rotation = value;
    }

    public bool IsGrounded => _controller.isGrounded;
    public Vector3 Up => _pivot.up;
    public Vector3 Forward => _pivot.forward;
    public Vector3 Right => _pivot.right;
    public Vector3 Velocity => _controller.velocity;
    public PlayerCollider Collider => _playerCollider.NotNull();

    public PlayerSettings Settings { get; private set; }
    public IInputManager InputManager { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public Transform CameraFocusPoint => _cameraFocusPoint.NotNull();
    public bool IsMovementBlocked => Animator.IsMovementBlocked;

    public void Construct(PlayerSettings settings, IInputManager inputManager)
    {
        InputManager = inputManager.NotNull();
        Settings = settings.NotNull();
    }

    private void Awake()
    {
        InputManager = EmptyInputManager.Instance;
        StateMachine = GetComponent<PlayerStateMachine>().NotNull();
        _controller = GetComponent<CharacterController>().NotNull();
        Animator = GetComponent<PlayerAnimator>().NotNull();
        Data = GetComponent<PlayerData>().NotNull();
        Debug.Assert(gameObject.layer == Data.Layer);
    }

    private void OnDestroy()
    {
        Destroying?.Invoke(this);
        Destroying = null;
    }

    public void Move(Vector3 motion)
    {
        _controller.Move(motion);
    }

    public void EnableMovement(bool enable)
    {
        if (enable)
            StateMachine.SetState(PlayerState.Walk);
        else
            StateMachine.SetState(PlayerState.None);

        // Animator.SetIsNoneState(enable); todo
    }
}