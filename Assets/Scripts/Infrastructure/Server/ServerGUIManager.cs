using System;
using Infrastructure.GUI;
using Mirror;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerGUIManager : NetworkBehaviour
    {
        [SerializeField] private ClientGUIManager _clientGUIManager;

        private StartGameButton _startGameButton;

        private void OnGUI()
        {
            _startGameButton?.Draw();
        }

        [ClientRpc]
        public void RpcShowGameOverPanel(string winningPlayerName, float durationSeconds) =>
            _clientGUIManager.ShowGameOverPanel(winningPlayerName, durationSeconds);

        [ClientRpc]
        public void RpcHideGameOverPanel() =>
            _clientGUIManager.HideGameOverPanel();

        [ClientRpc]
        public void RpcShowInGamePlayersScore() =>
            _clientGUIManager.ShowInGamePlayersScore();

        [ClientRpc]
        public void RpcHideInGamePlayersScore() =>
            _clientGUIManager.RpcHideInGamePlayersScore();

        [Server]
        public void ShowStartGameButton(Action onClick) =>
            _startGameButton = new StartGameButton(onClick);

        [Server]
        public void HideStartGameButton() =>
            _startGameButton = null;
    }
}
