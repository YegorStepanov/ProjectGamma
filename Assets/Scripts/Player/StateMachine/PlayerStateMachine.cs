﻿using System.Collections.Generic;
using Mirror;

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
        if (isLocalPlayer)
            State = PlayerState.Walk;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _states[State].Update();
    }

    public void SetState(PlayerState state)
    {
        if (!isLocalPlayer) return;

        _states[State].Exit();

        State = state;
        CmdSetState(state);

        _states[State].Enter();
    }

    [Command]
    private void CmdSetState(PlayerState state)
    {
        State = state;
    }
}