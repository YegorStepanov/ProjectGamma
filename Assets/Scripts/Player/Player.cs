using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerData))]
public sealed class Player : NetworkBehaviour, IPlayer
{
    public event Action<Player, ControllerColliderHit> Hit;
    public event Action<Player> LocalPlayerStarted;
    public event Action<Player> Destroying;

    public IPlayerData Data { get; set; }
    public IStateMachine<PlayerState> StateMachine { get; private set; }

    [SerializeField] private Transform _cameraFocusPoint;
    [Tooltip("Child transform without CharacterController because it doesn't support Y rotation")]
    [SerializeField] private Transform Pivot;

    private CharacterController _controller;

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            if (_controller.enabled)
                _controller.enabled = false;

            transform.position = value;

            if (!_controller.enabled)
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

    public PlayerSettings Settings { get; private set; }
    public IInputManager InputManager { get; private set; }
    public Transform CameraFocusPoint => _cameraFocusPoint.NotNull();

    #region TODO
    [ShowInInspector] public float RotSpeed;
    [ShowInInspector] public bool isNonNull;
    private void Update() => isNonNull = Settings != null;
    #endregion

    public void Construct(PlayerSettings settings, IInputManager inputManager)
    {
        Debug.Log($"Construct {inputManager.GetType().Name}");
        InputManager = inputManager.NotNull();
        Settings = settings.NotNull();
    }

    private void Awake()
    {
        InputManager = EmptyInputManager.Instance;
        StateMachine = GetComponent<PlayerStateMachine>().NotNull();
        _controller = GetComponent<CharacterController>().NotNull();
        Data = GetComponent<PlayerData>().NotNull();

        _controller.enabled = false;
    }

    public override void OnStartClient()
    {
        // RotSpeed = Settings.HorizontalRotationSpeedRadians;

        GameFactory gameFactory = GameObject.FindObjectOfType<GameFactory>();


        Hit += gameFactory.OnHit;
        LocalPlayerStarted += gameFactory.OnLocalPlayerStarted;

        OnStopClient();
    }

    public override void OnStartLocalPlayer()
    {
        _controller.enabled = true;
        LocalPlayerStarted?.Invoke(this);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Hit?.Invoke(this, hit);
    }

    private void OnDestroy()
    {
        Destroying?.Invoke(this);

        Hit = null;
        LocalPlayerStarted = null;
        Destroying = null;
    }

    public void Move(Vector3 motion)
    {
        _controller.Move(motion);
    }
}