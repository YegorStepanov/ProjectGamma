using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(WalkState))]
[RequireComponent(typeof(DashState))]
public sealed class PlayerStateMachine : NetworkBehaviour, IStateMachine<PlayerState>
{
    [field: SyncVar(hook = nameof(SetState))]
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
        if (!isLocalPlayer)
        {
            State = state;
            return;
        }

        _states[State].Exit();
        State = state;
        _states[State].Enter();
    }

    private void SetState(PlayerState _, PlayerState newState) =>
        SetState(newState);
}
