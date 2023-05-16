using Infrastructure.GUI;
using UnityEngine;
using GUISettings = Infrastructure.GUI.GUISettings;

namespace Infrastructure
{
    public sealed class ClientGUIManager : MonoBehaviour
    {
        [SerializeField] private ClientRoomManager _clientRoomManager;
        [SerializeField] private GUISettings _settings;

        private InGamePlayersData _inGamePlayersData;
        private GameOverPanel _gameOverPanel;

        private void OnGUI()
        {
            _inGamePlayersData?.Draw();
            _gameOverPanel?.Draw();
        }

        private void Update()
        {
            _gameOverPanel?.Update();
        }

        public void ShowGameOverPanel(string winningPlayerName, float durationSeconds) =>
            _gameOverPanel = new GameOverPanel(winningPlayerName, _settings.GameOverPanelWidth, _settings.GameOverPanelHeight, durationSeconds);

        public void HideGameOverPanel() =>
            _gameOverPanel = null;

        public void ShowInGamePlayersScore() =>
            _inGamePlayersData = new InGamePlayersData(_clientRoomManager.PlayerDatas);

        public void RpcHideInGamePlayersScore() =>
            _inGamePlayersData = null;
    }
}