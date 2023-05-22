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

    [SerializeField] private Transform _cameraFocusPoint;
    [SerializeField] private PlayerCollider _playerCollider;
    [SerializeField] private Transform _hitPlace;

    private Transform _transform;
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
        get => _transform.rotation;
        set => _transform.rotation = value;
    }

    public PlayerData Data { get; set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public bool IsGrounded => _controller.isGrounded;
    public Vector3 Up => _transform.up;
    public Vector3 Forward => _transform.forward;
    public Vector3 Right => _transform.right;
    public Vector3 Velocity => _controller.velocity;
    public PlayerCollider Collider => _playerCollider.NotNull();

    public PlayerSettings Settings { get; private set; }
    public IInputManager InputManager { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public Transform CameraFocusPoint => _cameraFocusPoint.NotNull();
    public Vector3 HitPlace => _hitPlace.position;
    public bool IsMovementBlocked => Animator.IsMovementBlocked;

    public void Construct(PlayerSettings settings, IInputManager inputManager)
    {
        InputManager = inputManager.NotNull();
        Settings = settings.NotNull();
    }

    private void Awake()
    {
        _transform = transform;
        InputManager = EmptyInputManager.Instance;
        StateMachine = GetComponent<PlayerStateMachine>().NotNull();
        _controller = GetComponent<CharacterController>().NotNull();
        Animator = GetComponent<PlayerAnimator>().NotNull();
        Data = GetComponent<PlayerData>().NotNull();
        Debug.Assert(gameObject.layer == Data.Layer);
    }

    public override void OnStartClient() =>
        Collider.enabled = false;

    public override void OnStartServer() =>
        Collider.enabled = true;

    private void OnDestroy()
    {
        Destroying?.Invoke(this);
        Destroying = null;
    }

    public void EnableMovement(bool enable)
    {
        if (enable)
            StateMachine.SetState(PlayerState.Walk);
        else
            StateMachine.SetState(PlayerState.None);
    }

    public void Move(Vector3 moveVelocity)
    {
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(moveVelocity, Up);

        bool isVelocityZero = horizontalVelocity.sqrMagnitude < Settings.MoveThreshold * Settings.MoveThreshold;
        if (!isVelocityZero)
        {
            RotateHorizontal(horizontalVelocity);
        }

        _controller.Move(moveVelocity * Time.deltaTime);
    }

    private void RotateHorizontal(Vector3 horizontalDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(horizontalDirection, Up);
        Rotation = Quaternion.RotateTowards(Rotation, lookRotation, Settings.RotationSpeed * Time.deltaTime);
    }
}