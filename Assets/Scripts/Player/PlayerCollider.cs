using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public sealed class PlayerCollider : MonoBehaviour
{
    public const string Tag = "PlayerCollider";

    public event Action<Player, Collider> CollisionEntered;

    [SerializeField] private Player _player;

    public Player Player => _player.NotNull();

    private void OnTriggerEnter(Collider other)
    {
        CollisionEntered?.Invoke(_player, other);
    }

    private void OnDestroy()
    {
        CollisionEntered = null;
    }
}