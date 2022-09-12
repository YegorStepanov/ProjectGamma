using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(CharacterController))]
public sealed class Player : NetworkBehaviour, IPlayer
{
    public const string Tag = "Player";

    [SerializeField] private Transform _pivot; //camera focus

    private PlayerStateMachine _stateMachine;
    public CharacterController _controller;

    private Func<Transform, ICameraController> _createCamera;
    private Action<Player, ControllerColliderHit> _onColliderHit;

    private Vector3 _moveDirection;

    public PlayerData Data { get; private set; }
    public PlayerState State => _stateMachine.State;

    [field: SyncVar]
    public uint Score { get; set; }
    [field: SyncVar]
    public string Name { get; set; }

    public IInputManager InputManager { get; private set; }
    private ICameraController Camera { get; set; }

    private void OnValidate()
    {
        Debug.Assert(CompareTag(Tag));
    }

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<CharacterController>();
        Camera = new MockCameraController(transform); //!!
    }

    public void Construct(
        PlayerData data,
        IInputManager inputManager,
        Func<Transform, ICameraController> createCamera,
        Action<Player, ControllerColliderHit> onColliderHit)
    {
        Data = data;
        InputManager = inputManager;
        _createCamera = createCamera;
        _onColliderHit = onColliderHit;
    }

    public Vector3 TransformDirection(Vector3 direction) =>
        Camera != null ? Camera.TransformDirection(direction) : transform.position;

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
        _controller.Move(motion);
    }

    public override void OnStartLocalPlayer()
    {
        Camera = _createCamera(_pivot); //!!!
        Debug.Log("OnStartLocalPlayer");
    }

    private void Start() { } //or here setstate should be?

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _onColliderHit(this, hit);
        _moveDirection = hit.moveDirection * hit.moveLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _moveDirection);
    }
}
