using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WalkState))]
[RequireComponent(typeof(DashState))]
public sealed class PlayerStateMachine : MonoBehaviour, IStateMachine<PlayerState>
{
    public PlayerState State { get; private set; }

    private Dictionary<PlayerState, IState> _states;

    private void Awake()
    {
        _states = new Dictionary<PlayerState, IState>
        {
            [PlayerState.None] = null,
            [PlayerState.Dash] = GetComponent<DashState>(),
            [PlayerState.Walk] = GetComponent<WalkState>()
        };
    }

    public void SetState(PlayerState state)
    {
        if (_states[State] != null)
            _states[State].Exit();

        State = state;
        IState newState = _states[state];

        if (newState != null)
            newState.Enter();
    }
}
