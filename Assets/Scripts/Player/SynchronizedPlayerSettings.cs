using Mirror;
using UnityEngine;

public sealed class SynchronizedPlayerSettings : NetworkBehaviour
{
    [SerializeField, SyncVar]
    private PlayerSettings _playerSettings;

    public PlayerSettings PlayerSettings
    {
        get => _playerSettings;
        set => _playerSettings = value;
    }
}