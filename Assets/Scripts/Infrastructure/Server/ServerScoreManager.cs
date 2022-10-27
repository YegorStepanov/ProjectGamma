using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure
{
    // public sealed class ClientScoreManager : NetworkBehaviour
    // {
    //     public
    // }

    public sealed class ServerScoreManager : NetworkBehaviour
    {
        [SerializeField] private RoomSettings _settings;
        // [SerializeField] private ClientScoreManager _clientScoreManager;

        public bool IsPlayerWon(IPlayer player)
        {
            return player.Data.ScoreData.Score >= _settings.PointsToWin;
        }

        public void IncreaseScore(IPlayer player)
        {
            player.Data.ScoreData.Score++;
        }
    }
}