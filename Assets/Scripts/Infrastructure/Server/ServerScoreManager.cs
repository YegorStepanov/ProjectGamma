using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerScoreManager : NetworkBehaviour
    {
        [SerializeField] private RoomSettings _settings;

        public bool IsPlayerWon(Player player)
        {
            return player.Data.Score >= _settings.PointsToWin;
        }

        public void IncreaseScore(Player player)
        {
            player.Data.Score++;
        }
    }
}