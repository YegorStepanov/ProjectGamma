using System;
using System.Collections.Generic;
using Mirror;
using Mirror.SimpleWeb;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerRoomManager : NetworkBehaviour
    {
        [SerializeField] private ClientRoomManager _clientRoomManager;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private ServerHeroCollisionManager _serverHeroCollisionManager;

        private FreeStartPositions _freeStartPositions;
        private int _botCounts;

        public RoomPlayers RoomPlayers { get; private set; }

        private void Update()
        {
            if (RoomPlayers?.Players?.Count != 2)
                return;
            // Debug.Log($"{RoomPlayers.Players.Count} ; {RoomPlayers.Players[1].Position}");
        }

        public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
        {
            _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
            RoomPlayers = new RoomPlayers(startPositions.Count);
            _botCounts = 0;
        }

        public void ReplaceAndConstructPlayer(NetworkConnectionToClient conn, GameObject gamePlayer, int playerIndex)
        {
            if (!isServer)
            {
                Debug.LogWarning("NOT SERVER!!!");
            }

            Player player = gamePlayer.GetComponent<Player>().NotNull();
            // player.Hit += PlayerOnHit;
            if(isServer)
                player.Collider.CollisionEntered += HandlePlayersCollision;

            Debug.Log($"subscribe to Hit: {player.Data.Name} {isServer} {isClient}");

            PreparePlayerForGame(player, playerIndex);
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);

            _clientRoomManager.TargetConstructPlayer(conn, gamePlayer, _playerSettings);
        }

        // Players
        private void HandlePlayersCollision(Player player, Collider collidedBody)
        {
            // Debug.Log($"wtf collided {player.name} -> {collidedBody.name}");

            if (!collidedBody.CompareTag(PlayerCollider.Tag)) //.transform?
                return;

            // Debug.Log($"CompareTag {player.name}");

            if (collidedBody.TryGetComponent(out PlayerCollider collidedCollider))
            {
                // Debug.Log($"TryGetComponent {player.name}");
                Player collidedPlayer = collidedCollider.Player;
                _serverHeroCollisionManager.HandleColliderHit(player, collidedPlayer);
            }
        }

        public void RestartGame()
        {
            PreparePlayersForGame();
        }

        private void PreparePlayersForGame()
        {
            _freeStartPositions.Reset();
            foreach (Player player in RoomPlayers.Players)
            {
                Vector3 position = _freeStartPositions.Pop();
                RoomPlayers.PreparePlayerToPlay(player, position, player.Data.Name);
            }
        }

        private void PreparePlayerForGame(Player player, int playerIndex)
        {
            //it works!
            Vector3 position = _freeStartPositions.Pop();
            string playerName = $"Player {playerIndex + 1}";
            RoomPlayers.PreparePlayerToPlay(player, position, playerName);
        }

        public Player CreatePlayer(Player playerPrefab)
        {
            Player player = Instantiate(playerPrefab);
            RoomPlayers.AddPlayer(player);
            _clientRoomManager.AddPlayer(player);
            return player;
        }

        public void RemovePlayer(Player player)
        {
            RoomPlayers.RemovePlayer(player);
            _clientRoomManager.RemovePlayer(player);
        }
    }
}