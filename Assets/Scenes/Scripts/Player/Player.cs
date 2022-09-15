using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(CharacterController))]
public sealed class Player : NetworkBehaviour, IPlayer
{
    public const string Tag = "Player";
    public static int Layer { get; private set; }

    public event Action<Player, ControllerColliderHit> Hit;
    public event Action<Player> LocalPlayerStarted;
    public event Action<Player> InfoChanged;
    public event Action<Player> Destroying;

    [SerializeField] private Transform _cameraFocusPoint;
    [SerializeField] private Renderer _renderer;

    [SyncVar]
    private string _name;

    [SyncVar]
    private uint _score;

    [SyncVar(hook = nameof(SetColor))]
    private Color32 _color;

    private PlayerStateMachine _stateMachine;
    private CharacterController _controller;
    private Material _material;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            InfoChanged?.Invoke(this);
        }
    }

    public uint Score
    {
        get => _score;
        set
        {
            _score = value;
            InfoChanged?.Invoke(this);
        }
    }

    public Color32 Color
    {
        get => _color;
        set
        {
            _color = value;
            InfoChanged?.Invoke(this);
        }
    }

    public PlayerData Data { get; private set; }
    public IInputManager InputManager { get; private set; }
    public PlayerState State => _stateMachine.State;
    public Transform CameraFocusPoint => _cameraFocusPoint;

    #region Unity
    #region TODO
    [ShowInInspector] public bool isNonNull;

    private void Update()
    {
        if (isNonNull == false && Data == null)
        {
            Debug.Log($"Getting null {name}");
        }

        isNonNull = Data != null;
    }
    #endregion

    private void Awake()
    {
        Layer = LayerMask.NameToLayer("Player");
        Debug.Assert(gameObject.CompareTag(Tag));
        Debug.Assert(gameObject.layer == Layer);

        _material = _renderer.material;
        _stateMachine = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<CharacterController>();
        _controller.enabled = false;
    }

    public void Construct(PlayerData data, IInputManager inputManager)
    {
        Data = data;
        InputManager = inputManager;
    }

    public override void OnStartLocalPlayer()
    {
        _controller.enabled = true;
        LocalPlayerStarted?.Invoke(this);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Hit?.Invoke(this, hit);
        // _moveDirection = hit.moveDirection * hit.moveLength;
    }

    private void OnDestroy()
    {
        Destroying?.Invoke(this);

        Destroying = null;
        Hit = null;
        LocalPlayerStarted = null;
        Destroy(_material);
    }
    #endregion

    public void SetRotation(Quaternion rotation)
    {
        //characterController cannot be rotated vertically, so rotate player model only
        transform.rotation = rotation;
    }

    public void SetState(PlayerState state) =>
        _stateMachine.SetState(state);

    public void SetPosition(Vector3 position)
    {
        _controller.enabled = false;
        transform.position = position;
        _controller.enabled = true;
    }

    public void Move(Vector3 motion)
    {
        _moveDirection = motion;
        _controller.Move(motion);
    }

    private void SetColor(Color32 _, Color32 newColor) =>
        _material.color = newColor;

    private Vector3 _moveDirection;

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawRay(transform.position, _moveDirection);
    }
}
