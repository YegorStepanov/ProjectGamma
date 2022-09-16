using UnityEngine;

public sealed class ScoreManager : Mirror.NetworkBehaviour
{
    [SerializeField] private RoomData _data;

    public bool IsPlayerWon(IPlayer player)
    {
        return player.Data.Score >= _data.PointsToWin;
    }

    public void IncreaseScore(IPlayer player)
    {
        player.Data.Score++;
    }
}
