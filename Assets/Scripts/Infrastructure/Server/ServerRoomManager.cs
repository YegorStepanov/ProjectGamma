﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerRoomManager : NetworkBehaviour
    {
        [SerializeField] private ClientRoomManager _clientRoomManager;
        [SerializeField] private ServerHeroCollisionManager _serverHeroCollisionManager;
        [SerializeField] private GameFactory _gameFactory;

        private FreeStartPositions _freeStartPositions;
        private int _botCounts;

        public RoomPlayers RoomPlayers { get; private set; }

        public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
        {
            _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
            RoomPlayers = new RoomPlayers(startPositions.Count);
            _botCounts = 0;
        }

        [Server]
        public Player CreatePlayer(Player playerPrefab)
        {
            Player player = Instantiate(playerPrefab);
            RoomPlayers.AddPlayer(player);
            _clientRoomManager.AddPlayerData(player);
            return player;
        }

        [Server]
        public void RemovePlayer(Player player)
        {
            RoomPlayers.RemovePlayer(player);
            _clientRoomManager.RemovePlayerData(player);
        }

        [Server]
        public void ReplaceAndConstructPlayer(NetworkConnectionToClient conn, GameObject gamePlayer, int playerIndex)
        {
            Player player = gamePlayer.GetComponent<Player>().NotNull();
            player.Collider.CollisionEntered += HandlePlayersCollisions;

            SetUpPlayer(player, playerIndex);
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);

            TargetConstructPlayer(conn, gamePlayer);
        }

        [Server]
        public void RestartGame()
        {
            _freeStartPositions.Reset();
            foreach (Player player in RoomPlayers.Players)
            {
                Vector3 position = _freeStartPositions.Pop();
                SetInitialPlayerData(player, position, player.Data.Name);
                TargetSetInitialPlayerData(player.connectionToClient, player, position, player.Data.Name);
            }
        }

        private void HandlePlayersCollisions(Player player, Collider collidedBody)
        {
            if (!collidedBody.CompareTag(PlayerCollider.Tag))
                return;

            if (collidedBody.TryGetComponent(out PlayerCollider collidedCollider))
            {
                Player collidedPlayer = collidedCollider.Player;
                _serverHeroCollisionManager.HandleColliderHit(player, collidedPlayer);
            }
        }

        private void SetUpPlayer(Player player, int playerIndex)
        {
            Vector3 position = _freeStartPositions.Pop();
            string playerName = $"Player {playerIndex + 1}";
            SetInitialPlayerData(player, position, playerName);
        }

        private void SetInitialPlayerData(Player player, Vector3 position, string playerName) =>
            RoomPlayers.SetInitialPlayerData(player, position, playerName);

        [TargetRpc]
        private void TargetSetInitialPlayerData([UsedImplicitly] NetworkConnection target, Player player, Vector3 position, string playerName) =>
            RoomPlayers.SetInitialPlayerData(player, position, playerName);

        [TargetRpc]
        private void TargetConstructPlayer([UsedImplicitly] NetworkConnection conn, GameObject playerGameObject) =>
            _gameFactory.ConstructPlayer(playerGameObject);
    }
}