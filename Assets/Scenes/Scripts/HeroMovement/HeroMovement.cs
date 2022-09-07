using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WalkState))]
[RequireComponent(typeof(DashState))]
public sealed class HeroMovement : MonoBehaviour, IStateMachine
{
    private IState _activeState;
    private Dictionary<Type, IState> _typeToState;

    public void Construct(IEnumerable<IState> states)
    {
        _typeToState = states.ToDictionary(s => s.GetType());
    }

    // private void Awake()
    // {
    //     _typeToState = GetComponents<IState>().ToDictionary(s => s.GetType());
    // }

    private void Start()
    {
        SetState<WalkState>();
    }

    public void SetState<T>() where T : IState
    {
        // ReSharper disable once UseNullPropagation
        if (_activeState != null)
            _activeState.Exit();

        _activeState = _typeToState[typeof(T)];
        _activeState.Enter();
    }
}
