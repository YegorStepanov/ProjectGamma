﻿using System;
using Infrastructure.GUI;
using Mirror;
using UnityEngine;
using GUISettings = Infrastructure.GUI.GUISettings;

namespace Infrastructure
{
    public sealed class GUIManager : NetworkBehaviour
    {
        [SerializeField] private ClientRoomManager _clientRoomManager;
        [SerializeField] private GUISettings _settings;

        private InGamePlayersData _inGamePlayersData;
        private GameOverPanel _gameOverPanel;
        private StartGameButton _startGameButton;

        [ServerCallback]
        private void OnGUI()
        {
            _inGamePlayersData?.Draw();
            _gameOverPanel?.Draw();
            _startGameButton?.Draw();
        }

        [ServerCallback]
        private void Update()
        {
            _gameOverPanel?.Update();
        }

        [ClientRpc]
        public void RpcShowGameOverPanel(string winningPlayerName, float durationSeconds) =>
            _gameOverPanel = new GameOverPanel(winningPlayerName, _settings.GameOverPanelWidth, _settings.GameOverPanelHeight, durationSeconds);

        [ClientRpc]
        public void RpcHideGameOverPanel() =>
            _gameOverPanel = null;

        [ClientRpc]
        public void RpcShowInGamePlayersScore() =>
            _inGamePlayersData = new InGamePlayersData(_clientRoomManager.PlayerDatas);

        [ClientRpc]
        public void RpcHideInGamePlayersScore() =>
            _inGamePlayersData = null;

        [Server]
        public void ShowStartGameButton(Action onClick) =>
            _startGameButton = new StartGameButton(onClick);

        [Server]
        public void HideStartGameButton() =>
            _startGameButton = null;
    }
}