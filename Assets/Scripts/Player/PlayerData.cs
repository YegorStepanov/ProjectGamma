using System;
using Mirror;
using UnityEngine;

public sealed class PlayerData : NetworkBehaviour, IPlayerData
{
    public event Action<IPlayer> Changed;

    [SyncVar(hook = nameof(RaiseChanged))]
    private PlayerScoreData _scoreData;
    [SyncVar(hook = nameof(RaiseChanged))]
    private Color32 _color;

    private IPlayer _player;

    public string Tag => gameObject.tag;
    public int Layer => gameObject.layer;

    public PlayerScoreData ScoreData
    {
        get => _scoreData;
        set
        {
            _scoreData = value;
            Changed?.Invoke(_player);
        }
    }

    public Color32 Color
    {
        get => _color;
        set
        {
            _color = value;
            Changed?.Invoke(_player);
        }
    }

    private void Awake()
    {
        _scoreData = new PlayerScoreData();
        _player = GetComponent<IPlayer>().NotNull();
        Debug.Assert(gameObject.CompareTag(Tag));
        Debug.Assert(gameObject.layer == Layer);
    }

    private void OnDestroy() =>
        Changed = null;

    private void RaiseChanged(PlayerScoreData _, PlayerScoreData newData) =>
        Changed?.Invoke(_player);

    private void RaiseChanged(Color32 _, Color32 newColor) =>
        Changed?.Invoke(_player);
}

public class PlayerScoreData
{
    [SyncVar]
    public string Name;
    [SyncVar]
    public uint Score;
}