using Mirror;
using UnityEngine;

namespace Infrastructure.GUI
{
    public sealed class InGamePlayersScore // -> InGamePlayerData
    {
        private readonly SyncList<PlayerScoreData> _playerDatas;

        public InGamePlayersScore(SyncList<PlayerScoreData> playerDatas) =>
            _playerDatas = playerDatas;

        public void Draw()
        {
            int index = 0;
            foreach (PlayerScoreData data in _playerDatas)
            {
                DrawScore(index, data);
                index++;
            }
        }

        private static void DrawScore(int index, PlayerScoreData data)
        {
            UnityEngine.GUI.Box(new Rect(10f + index * 90, 10f, 80f, 25f), $"{data.Name}: {data.Score:0}");
        }
    }
}