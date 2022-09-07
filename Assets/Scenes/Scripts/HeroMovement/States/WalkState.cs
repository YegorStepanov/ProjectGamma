using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class WalkState : MonoBehaviour, IState
{
    [Range(0, 100)]
    [SerializeField] private float _speed;

    private CharacterController _controller;
    private IStateMachine _stateMachine;

    private IInputManager _inputManager;
    private Transform _relativeTo;

    public void Construct(IInputManager inputManager, Transform relativeTo)
    {
        _inputManager = inputManager;
        _relativeTo = relativeTo;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stateMachine = GetComponent<IStateMachine>();
        enabled = false;
    }

    public void Enter() =>
        enabled = true;

    public void Exit() =>
        enabled = false;

    private void Update()
    {
        if (_inputManager.ReadJumpAction())
        {
            _stateMachine.SetState<DashState>();
            return;
        }

        Vector3 input = ReadInput();
        Vector3 direction = GetDirection(input);
        _controller.SimpleMove(direction * _speed);
    }

    private Vector3 ReadInput()
    {
        Vector2 input = _inputManager.ReadMoveAction();
        return new Vector3(input.x, 0, input.y);
    }

    private Vector3 GetDirection(Vector3 input)
    {
        Vector3 direction = _relativeTo.TransformDirection(input);
        //mb we should project it on this.forward?
        return direction;
    }
}
