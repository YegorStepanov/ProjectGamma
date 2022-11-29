using Mirror;
using UnityEngine;

namespace Infrastructure.GUI
{
    public sealed class InGamePlayersData
    {
        private readonly SyncList<PlayerData> _playersData;

        public InGamePlayersData(SyncList<PlayerData> playersData) =>
            _playersData = playersData;

        public void Draw()
        {
            int index = 0;
            foreach (PlayerData data in _playersData)
            {
                DrawScore(index, data);
                index++;
            }
        }

        private static void DrawScore(int index, PlayerData data)
        {
            UnityEngine.GUI.Box(new Rect(10f + index * 90, 10f, 80f, 25f), $"{data.Name}: {data.Score:0}");
        }
    }
}