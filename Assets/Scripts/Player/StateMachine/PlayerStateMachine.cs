using System.Collections.Generic;
using Mirror;

public sealed class PlayerStateMachine : NetworkBehaviour, IStateMachine<PlayerState>
{
    [field: SyncVar(hook = nameof(SetState))]
    public PlayerState State { get; private set; }

    private Dictionary<PlayerState, IPlayerState> _states;

    private void Awake()
    {
        var player = GetComponent<IPlayer>();

        var functions = new PlayerStateFunctions(player);

        _states = new Dictionary<PlayerState, IPlayerState>
        {
            [PlayerState.None] = NoneState.Instance,
            [PlayerState.Dash] = new DashState(player, functions),
            [PlayerState.Walk] = new WalkState(player, functions)
        };

        State = PlayerState.None;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _states[State].Update();
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
        _states[State] = _states[State];

        _states[State].Enter();
    }

    private void SetState(PlayerState _, PlayerState state) =>
        SetState(state);
}
