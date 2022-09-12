using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Players : IEnumerable<Player>
{
    private readonly List<Player> _players;

    public Players(int maxPlayers)
    {
        _players = new List<Player>(maxPlayers);
    }

    public void AddPlayer(Player player) =>
        _players.Add(player);

    public void RemovePlayer(Player player) => //onDisconnect
        _players.Remove(player);

    public void ResetScores()
    {
        foreach (Player player in _players)
            player.Score = 0;
    }

    public void SetStatesToNone()
    {
        foreach (Player player in _players)
            player.SetState(PlayerState.None);
    }

    public void SetStatesToWalk()
    {
        foreach (Player player in _players)
            player.SetState(PlayerState.Walk);
    }

    public IEnumerator<Player> GetEnumerator() =>
        _players.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public void ResetPositions(FreeStartPositions freeStartPositions)
    {
        foreach (Player player in _players)
        {
            Vector3 position = freeStartPositions.Pop();
            player.SetPosition(position);
        }
    }
}
