using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerBlockingManager : NetworkBehaviour
    {
        [SerializeField] private RoomSettings _settings;

        private HashSet<Player> _blockedPlayers;

        private void Awake()
        {
            _blockedPlayers = new HashSet<Player>();
        }

        public bool TryBlock(Player player)
        {
            if (IsBlocked(player))
                return false;

            _blockedPlayers.Add(player);
            StartCoroutine(WaitAndUnblock(player));
            return true;
        }

        private bool IsBlocked(Player player)
        {
            return _blockedPlayers.Contains(player);
        }

        private IEnumerator WaitAndUnblock(Player player)
        {
            TargetEnableMovement(player.connectionToClient, player, false);
            yield return new WaitForSecondsRealtime(_settings.BlockingTime);
            TargetEnableMovement(player.connectionToClient, player, true);
            _blockedPlayers.Remove(player);
        }

        [TargetRpc]
        private void TargetEnableMovement([UsedImplicitly] NetworkConnectionToClient conn, Player player, bool enableMovement)
        {
            player.EnableMovement(enableMovement);
        }
    }
}