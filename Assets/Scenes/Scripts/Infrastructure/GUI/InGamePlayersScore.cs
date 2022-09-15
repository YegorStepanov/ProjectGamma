using UnityEngine;

public sealed class InGamePlayersScore
{
    private readonly RoomPlayers _roomPlayers;

    public InGamePlayersScore(RoomPlayers roomPlayers) =>
        _roomPlayers = roomPlayers;

    public void Draw()
    {
        int index = 0;
        foreach (IPlayer player in _roomPlayers.Players)
        {
            DrawScore(player, index);
            index++;
        }
    }

    private static void DrawScore(IPlayer player, int index)
    {
        GUI.Box(new Rect(10f + index * 90, 10f, 80f, 25f), $"{player.Name}: {player.Score:0}");
    }
}
