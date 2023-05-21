using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public sealed class RoomPlayers
    {
        private readonly List<Player> _players;

        public IReadOnlyList<Player> Players => _players;

        public RoomPlayers(int maxPlayers)
        {
            _players = new List<Player>(maxPlayers);
        }

        public void AddPlayer(Player player)
        {
            player.Destroying += OnDestroying;
            _players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            player.Destroying -= OnDestroying;
            _players.Remove(player);
        }

        private void OnDestroying(Player player)
        {
            _players.Remove(player);
        }
    }
}