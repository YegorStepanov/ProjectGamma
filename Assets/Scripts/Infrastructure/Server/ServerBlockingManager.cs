using System.Collections;
using System.Collections.Generic;
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
            // Debug.Log("awake");
            _blockedPlayers = new HashSet<Player>();
        }

        private bool IsBlocked(Player player)
        {
            return _blockedPlayers.Contains(player);
        }

        public bool TryBlock(Player player)
        {
            if (IsBlocked(player))
                return false;

            _blockedPlayers.Add(player);
            StartCoroutine(SetColorRoutine(player));
            return true;
        }

        private IEnumerator SetColorRoutine(Player player)
        {
            Color oldColor = player.Data.Color;

            player.Data.Color = _settings.BlockingColor;
            yield return new WaitForSeconds(_settings.BlockingTime);
            player.Data.Color = oldColor;

            _blockedPlayers.Remove(player);
        }
    }
}