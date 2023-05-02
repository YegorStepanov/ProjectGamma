using System;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public sealed class PlayerData : NetworkBehaviour
{
    public event Action<Player> Changed;

    [SyncVar(hook = nameof(RaiseChanged))] private string _name;
    [SyncVar(hook = nameof(RaiseChanged))] private uint _score;
    [SyncVar(hook = nameof(RaiseChanged))] private Color32 _color;

    private Player _player;

    public int Layer => gameObject.layer;

    [NotNull]
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
        _name = "";
        _player = GetComponent<Player>().NotNull();
    }

    private void OnDestroy() =>
        Changed = null;

    private void RaiseChanged(string _, string __)
    {
        RaiseChanged();
    }

    private void RaiseChanged(uint _, uint __)
    {
        RaiseChanged();
    }

    private void RaiseChanged(Color32 _, Color32 __)
    {
        Debug.Log($"Changed Color {__}");
        RaiseChanged();
    }

    private void RaiseChanged() =>
        Changed?.Invoke(_player);
}