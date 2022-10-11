using UnityEngine;

public sealed class ScoreManager : Mirror.NetworkBehaviour
{
    [SerializeField] private RoomSettings _settings;

    public bool IsPlayerWon(IPlayer player)
    {
        return player.Data.Score >= _settings.PointsToWin;
    }

    public void IncreaseScore(IPlayer player)
    {
        player.Data.Score++;
    }
}