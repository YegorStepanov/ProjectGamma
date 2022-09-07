using UnityEngine;

public sealed class DashState : MonoBehaviour, IState
{
    [SerializeField] private float _distance;
    [Range(0f, 100f)]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private AnimationCurve _speed;

    private CharacterController _controller;
    private IStateMachine _stateMachine;

    private float _distanceSqr;
    private float _remainingDistance;

    private Vector3 _direction;
    private Vector3 _startPosition;

    private Transform _relativeTo;

    public void Construct(Transform relativeTo, IStateMachine stateMachine)
    {
        _relativeTo = relativeTo;
        _stateMachine = stateMachine;
    }

    private void OnValidate() =>
        _distanceSqr = _distance * _distance;

    private void Awake()
    {
        OnValidate();
        _controller = GetComponent<CharacterController>();
        _stateMachine = GetComponent<IStateMachine>();
        enabled = false;
    }

    void IState.Enter()
    {
        enabled = true;

        _direction = _relativeTo.TransformDirection(Vector3.forward); // transform.forward;
        _startPosition = transform.position;

        _remainingDistance = _distance;
    }

    void IState.Exit()
    {
        enabled = false;
    }

    private void Update()
    {
        if (NeedToMove()) //revert?
        {
            _stateMachine.SetState<WalkState>();
            return;
        }

        float speed = GetSpeed();
        _controller.SimpleMove(_direction * speed);

        _remainingDistance -= speed * Time.deltaTime;
    }

    private bool NeedToMove()
    {
        Vector3 delta = transform.position - _startPosition;
        return delta.sqrMagnitude >= _distanceSqr;
    }

    private float GetSpeed()
    {
        float percentage = _remainingDistance / _distance;
        float speed = _speed.Evaluate(percentage) * _maxSpeed;
        return speed;
    }
}
