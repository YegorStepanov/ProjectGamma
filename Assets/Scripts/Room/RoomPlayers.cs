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

        public void EnableMovingForAll()
        {
            foreach (Player player in _players)
                player.StateMachine.SetState(PlayerState.Walk);
        }

        public void DisableMovingForAll()
        {
            //todo: call rpc! here
            foreach (Player player in _players)
                player.StateMachine.SetState(PlayerState.None);
        }

        public void PreparePlayerToPlay(Player player, Vector3 position, string playerName)
        {
            player.transform.name = playerName;
            player.Position = position;
            player.Rotation = RotateToSceneCenter(position);
            player.Data.Score = 0;
            player.Data.Name = playerName;
            player.StateMachine.SetState(PlayerState.Walk);
        }

        private static Quaternion RotateToSceneCenter(Vector3 position)
        {
            Vector3 lookDirection = -position;
            lookDirection.y = 0;
            return Quaternion.LookRotation(lookDirection);
        }

        private void OnDestroying(Player player)
        {
            _players.Remove(player);
        }
    }
}