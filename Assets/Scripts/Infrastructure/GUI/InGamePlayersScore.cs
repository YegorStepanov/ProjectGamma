using Room;
using UnityEngine;

namespace Infrastructure.GUI
{
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
            UnityEngine.GUI.Box(new Rect(10f + index * 90, 10f, 80f, 25f), $"{player.Data.Name}: {player.Data.Score:0}");
        }
    }
}
