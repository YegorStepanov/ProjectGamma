using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WalkState))]
[RequireComponent(typeof(DashState))]
public sealed class PlayerStateMachine : Mirror.NetworkBehaviour, IStateMachine<PlayerState>
{
    public PlayerState State { get; private set; } = PlayerState.None;

    private Dictionary<PlayerState, IState> _states;

    private void Awake()
    {
        _states = new Dictionary<PlayerState, IState>
        {
            [PlayerState.None] = NoneState.Instance,
            [PlayerState.Dash] = GetComponent<DashState>(),
            [PlayerState.Walk] = GetComponent<WalkState>()
        };
    }
    
    public void SetState(PlayerState state)
    {
        _states[State].Exit();
        State = state;
        _states[State].Enter();
    }
}
