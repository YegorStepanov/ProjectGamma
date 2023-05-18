using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class PlayerStateMachine : NetworkBehaviour, IStateMachine<PlayerState>
{
    public PlayerState State { get; private set; }

    private Dictionary<PlayerState, IPlayerState> _states;

    private void Awake()
    {
        var player = GetComponent<Player>().NotNull();

        var functions = new PlayerStateFunctions(player);

        _states = new Dictionary<PlayerState, IPlayerState>
        {
            [PlayerState.None] = new NoneState(player),
            [PlayerState.Dash] = new DashState(player, functions),
            [PlayerState.Walk] = new WalkState(player, functions)
        };
    }

    private void Start()
    {
        if (!isLocalPlayer) return;

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
        Debug.Log($"update_state_on_server: old={State} new={state}");
        State = state;
    }
}