using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(CharacterController))]
public sealed class Player : NetworkBehaviour, IPlayer
{
    public const string Tag = "Player";

    [SerializeField] private Transform _pivot; //camera focus
    [SerializeField] private Renderer _renderer;

    [field: SyncVar]
    public string Name { get; set; }
    [field: SyncVar]
    public uint Score { get; set; }
    [field: SyncVar(hook = nameof(SetColor))]
    public Color32 Color { get; set; }

    private PlayerStateMachine _stateMachine;
    private CharacterController _controller;

    private Func<Transform, ICameraController> _createCamera;
    private Action<IPlayer, ControllerColliderHit> _onColliderHit;

    private Vector3 _moveDirection;
    private Material _material;

    public PlayerData Data { get; private set; }
    public PlayerState State => _stateMachine.State;

    public IInputManager InputManager { get; private set; }
    private ICameraController Camera { get; set; }

    private void OnValidate()
    {
        Debug.Assert(CompareTag(Tag));
    }

    private void Awake()
    {
        _material = _renderer.material;
        _stateMachine = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<CharacterController>();
        Camera = new MockCameraController(transform); //!!
    }

    public void Construct(
        PlayerData data,
        IInputManager inputManager,
        Func<Transform, ICameraController> createCamera,
        Action<IPlayer, ControllerColliderHit> onColliderHit)
    {
        Data = data;
        InputManager = inputManager;
        _createCamera = createCamera;
        _onColliderHit = onColliderHit;
        Debug.Log("1Player.Construct");
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("2Player.Construct");
        GameFactory.Instance.Construct(this);

        Camera = _createCamera(_pivot); //!!!
        SetState(PlayerState.Walk);
        // Camera = GameFactory.Instance.CreateCamera(_pivot);
        Debug.Log("OnStartLocalPlayer");
    }

    private void OnDestroy() =>
        Destroy(_material);

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _onColliderHit(this, hit);
        _moveDirection = hit.moveDirection * hit.moveLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawRay(transform.position, _moveDirection);
    }

    public void SetState(PlayerState state) =>
        _stateMachine.SetState(state);

    public void SetPosition(Vector3 position)
    {
        _controller.enabled = false;
        transform.position = position;
        _controller.enabled = true;
    }

    public void Move(Vector3 motion) =>
        _controller.Move(motion);

    public Vector3 TransformDirection(Vector3 direction) =>
        Camera != null ? Camera.TransformDirection(direction) : direction; //or transform.TransformDirection(sa)

    private void SetColor(Color32 _, Color32 newColor) =>
        _material.color = newColor;
}
