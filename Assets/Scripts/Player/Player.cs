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
    [Tooltip("Child transform without CharacterController because it doesn't support Y rotation")]
    [SerializeField] private Transform Pivot;
    [SerializeField] private PlayerCollider _playerCollider;

    private CharacterController _controller;

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            if (_controller.enabled)
                _controller.enabled = false;

            transform.position = value;

            _controller.enabled = true;
        }
    }

    public Quaternion Rotation
    {
        get => Pivot.rotation;
        set => Pivot.rotation = value;
    }

    public bool IsGrounded => _controller.isGrounded;
    public Vector3 Up => Pivot.up;
    public Vector3 Forward => Pivot.forward;
    public Vector3 Right => Pivot.right;
    public Vector3 Velocity => _controller.velocity; //is ok?
    public PlayerCollider Collider => _playerCollider.NotNull();

    public PlayerSettings Settings { get; private set; }
    public IInputManager InputManager { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public Transform CameraFocusPoint => _cameraFocusPoint.NotNull();

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
}