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

    public Transform RelativeMovementTo { get; private set; }

    private PlayerStateMachine _stateMachine;
    private CharacterController _controller;

    private Func<Transform, CameraController> _createCamera;
    private Action<IPlayer, ControllerColliderHit> _onColliderHit;

    private Material _material;
    private Func<IInputManager> _createInputManager;

    public PlayerData Data { get; private set; }
    public PlayerState State => _stateMachine.State;

    public IInputManager InputManager { get; private set; }

    [ShowInInspector]
    public bool isNonNull;

    #region Unity
    private void Update()
    {
        if (isNonNull == false && Data == null)
        {
            Debug.Log($"Getting null {name}");
        }

        isNonNull = Data != null;
    }

    private void Awake()
    {
        transform.tag = Tag;
        _material = _renderer.material;
        InputManager = new EmptyInputManager();

        _stateMachine = GetComponent<PlayerStateMachine>();
        _controller = GetComponent<CharacterController>();
        RelativeMovementTo = transform; //mb pivot?
        _controller.enabled = false;
    }

    public void Construct(
        PlayerData data,
        Func<IInputManager> createInputManager,
        Func<Transform, CameraController> createCamera,
        Action<IPlayer, ControllerColliderHit> onColliderHit)
    {
        Debug.Log($"isData null {data == null}");
        Data = data;
        _createInputManager = createInputManager;
        _createCamera = createCamera;
        _onColliderHit = onColliderHit;
    }

    public override void OnStartLocalPlayer()
    {
        if (_createCamera == null) return;

        RelativeMovementTo = _createCamera(_pivot).transform;
        InputManager = _createInputManager();
        _controller.enabled = true;

        SetState(PlayerState.Walk);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _onColliderHit(this, hit);
        _moveDirection = hit.moveDirection * hit.moveLength;
    }

    private void OnDestroy() =>
        Destroy(_material);
    #endregion

    public void SetRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
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

    private void SetColor(Color32 _, Color32 newColor) =>
        _material.color = newColor;

    private Vector3 _moveDirection;

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawRay(transform.position, _moveDirection);
    }
}
