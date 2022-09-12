using UnityEngine;

public sealed class InGamePlayersScore
{
    private readonly Players _players;

    public InGamePlayersScore(Players players) =>
        _players = players;

    public void Draw()
    {
        int index = 0;
        foreach (Player player in _players)
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
