﻿using System.Collections.Generic;
using UnityEngine;

public sealed class RoomPlayers
{
    private readonly List<IPlayer> _players;

    public IReadOnlyCollection<IPlayer> Players => _players;

    public RoomPlayers(int maxPlayers)
    {
        _players = new List<IPlayer>(maxPlayers);
    }

    public void AddPlayer(IPlayer player)
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
        foreach (IPlayer player in _players)
            player.StateMachine.SetState(PlayerState.Walk);
    }

    public void DisableMovingForAll()
    {
        foreach (IPlayer player in _players)
            player.StateMachine.SetState(PlayerState.None);
    }

    public void PreparePlayerToPlay(IPlayer player, Vector3 position, string playerName)
    {
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