using Room;
using UnityEngine;

namespace Infrastructure
{
    public sealed class ServerScoreManager : Mirror.NetworkBehaviour
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
}