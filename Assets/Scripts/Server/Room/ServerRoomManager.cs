﻿using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class ServerRoomManager : NetworkBehaviour
{
    [SerializeField] private ClientRoomManager _clientRoomManager;
    [SerializeField] private PlayerSettings _playerSettings;

    private FreeStartPositions _freeStartPositions;
    private int _botCounts;

    public RoomPlayers RoomPlayers { get; private set; }

    public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
    {
        _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
        RoomPlayers = new RoomPlayers(startPositions.Count);
        _botCounts = 0;
    }

    public void ReplaceAndConstructPlayer(NetworkConnectionToClient conn, GameObject gamePlayer, int playerIndex)
    {
        IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();
        PreparePlayerForGame(player, playerIndex);

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);

        _clientRoomManager.TargetConstructPlayer(conn, gamePlayer, _playerSettings);
    }

    public void RestartGame()
    {
        PreparePlayersForGame();
    }

    private void PreparePlayersForGame()
    {
        _freeStartPositions.Reset();
        foreach (IPlayer player in RoomPlayers.Players)
        {
            Vector3 position = _freeStartPositions.Pop();
            RoomPlayers.PreparePlayerToPlay(player, position, player.Data.Name);
        }
    }

    private void PreparePlayerForGame(IPlayer player, int playerIndex)
    {
        Vector3 position = _freeStartPositions.Pop();
        string playerName = $"Player {playerIndex + 1}";
        RoomPlayers.PreparePlayerToPlay(player, position, playerName);
    }

    public Player CreatePlayer(Player playerPrefab)
    {
        Player player = Instantiate(playerPrefab);
        RoomPlayers.AddPlayer(player);
        return player;
    }

    public void RemovePlayer(Player player)
    {
        RoomPlayers.RemovePlayer(player);
    }
}