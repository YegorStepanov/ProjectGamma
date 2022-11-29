using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class PlayerStateMachine : NetworkBehaviour, IStateMachine<PlayerState>
{
    public PlayerState State { get; private set; }

    private Dictionary<PlayerState, IPlayerState> _states;

    private void Awake()
    {
        var player = GetComponent<Player>();

        var functions = new PlayerStateFunctions(player);

        _states = new Dictionary<PlayerState, IPlayerState>
        {
            [PlayerState.None] = NoneState.Instance,
            [PlayerState.Dash] = new DashState(player, functions),
            [PlayerState.Walk] = new WalkState(player, functions)
        };
    }

    private void Start()
    {
        //set it on the server
        if (isLocalPlayer)
            State = PlayerState.Walk;
    }

    private void Update()
    {
        // !hasAuthority
        // if (!isServer) return;
        // if (!isLocalPlayer || !isServer) return;
        if (!isLocalPlayer) return;

        _states[State].Update();
    }

    public void SetState(PlayerState state)
    {
        if (!isLocalPlayer) return;

        _states[State].Exit();

        State = state;
        CmdSetState(state);
        //_states[State] = _states[State];

        _states[State].Enter();
    }

    [Command]
    private void CmdSetState(PlayerState state)
    {
        Debug.Log($"update_state_on_server: old={State} new={state}");
        State = state;
    }
}