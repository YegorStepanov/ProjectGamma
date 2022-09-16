using System;
using Mirror;
using UnityEngine;

public sealed class PlayerData : NetworkBehaviour, IPlayerData
{
    public event Action<IPlayer> Changed;

    [SyncVar(hook = nameof(RaiseChanged))]
    private string _name;
    [SyncVar(hook = nameof(RaiseChanged))]
    private uint _score;
    [SyncVar(hook = nameof(RaiseChanged))]
    private Color32 _color;

    private IPlayer _player;

    public string Tag => gameObject.tag;
    public int Layer => gameObject.layer;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Changed?.Invoke(_player);
        }
    }

    public uint Score
    {
        get => _score;
        set
        {
            _score = value;
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
        _player = GetComponent<IPlayer>().NotNull();
        Debug.Assert(gameObject.CompareTag(Tag));
        Debug.Assert(gameObject.layer == Layer);
    }

    private void OnDestroy() =>
        Changed = null;

    private void RaiseChanged(Color32 _, Color32 newColor) =>
        Changed?.Invoke(_player);

    private void RaiseChanged(uint _, uint newUint) =>
        Changed?.Invoke(_player);

    private void RaiseChanged(string _, string newStr) =>
        Changed?.Invoke(_player);
}
