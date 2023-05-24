using JetBrains.Annotations;
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

        public bool IsPlayerWon(Player player)
        {
            return player.Data.Score >= _settings.PointsToWin;
        }

        [UsedImplicitly]
        public void IncreaseScore(Player player)
        {
            player.Data.Score++;
        }
    }
}