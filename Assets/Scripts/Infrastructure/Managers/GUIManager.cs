using System;
using Mirror;
using UnityEngine;

public sealed class GUIManager : Mirror.NetworkBehaviour
{
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private GUISettings _settings;

    private InGamePlayersScore _inGamePlayersScore;
    private GameOverPanel _gameOverPanel;
    private StartGameButton _startGameButton;

    [ServerCallback]
    private void OnGUI()
    {
        _inGamePlayersScore?.Draw();
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
        _inGamePlayersScore = new InGamePlayersScore(_roomManager.RoomPlayers);

    [ClientRpc]
    public void RpcHideInGamePlayersScore() =>
        _inGamePlayersScore = null;

    [Server]
    public void ShowStartGameButton(Action onClick) =>
        _startGameButton = new StartGameButton(onClick);

    [Server]
    public void HideStartGameButton() =>
        _startGameButton = null;
}