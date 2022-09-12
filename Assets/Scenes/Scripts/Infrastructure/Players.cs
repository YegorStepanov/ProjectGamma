using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Players : IEnumerable<IPlayer>
{
    private readonly List<IPlayer> _players;

    public Players(int maxPlayers)
    {
        _players = new List<IPlayer>(maxPlayers);
    }

    public void AddPlayer(IPlayer player) =>
        _players.Add(player);

    public void RemovePlayer(IPlayer player) => //onDisconnect
        _players.Remove(player);

    public void ResetScores()
    {
        foreach (IPlayer player in _players)
            player.Score = 0;
    }

    public void SetStatesToNone()
    {
        foreach (IPlayer player in _players)
            player.SetState(PlayerState.None);
    }

    public void SetStatesToWalk()
    {
        foreach (IPlayer player in _players)
            player.SetState(PlayerState.Walk);
    }

    public IEnumerator<IPlayer> GetEnumerator() =>
        _players.GetEnumerator();

    public void ResetPositions(FreeStartPositions freeStartPositions)
    {
        foreach (IPlayer player in _players)
        {
            Vector3 position = freeStartPositions.Pop();
            player.SetPosition(position);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}
