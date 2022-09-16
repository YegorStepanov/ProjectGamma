using System.Collections.Generic;
using UnityEngine;

public sealed class RoomPlayers
{
    private readonly List<IPlayer> _players;

    public IEnumerable<IPlayer> Players => _players;
    public int Count => _players.Count;

    public RoomPlayers(int maxPlayers)
    {
        _players = new List<IPlayer>(maxPlayers);
    }

    public void AddPlayer(IPlayer player)
    {
        player.Destroying += OnDestroying;
        _players.Add(player);
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

    public void PreparePlayersForGame(FreeStartPositions positions)
    {
        foreach (IPlayer player in _players)
        {
            PreparePlayerForGame(player, positions);
        }
    }

    public void PreparePlayerForGame(IPlayer player, FreeStartPositions positions)
    {
        Vector3 position = positions.Pop();
        Quaternion rotation = RotationToSceneCenter(position); //?

        // player.SetState(PlayerState.None); //?
        player.Position = position;
        player.Rotation = rotation;
        player.Data.Score = 0;
        player.StateMachine.SetState(PlayerState.Walk);
    }

    private static Quaternion RotationToSceneCenter(Vector3 position)
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
