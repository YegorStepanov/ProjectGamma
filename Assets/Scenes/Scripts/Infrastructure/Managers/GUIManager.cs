using System;
using Mirror;
using UnityEngine;

public sealed class GUIManager : Mirror.NetworkBehaviour
{
    [SerializeField] private ServerRoomManager _serverRoomManager;
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private int width = 160;
    [SerializeField] private int height = 100;

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
        _gameOverPanel = new GameOverPanel(winningPlayerName, width, height, durationSeconds);

    [ClientRpc]
    public void RpcHideGameOverPanel() =>
        _gameOverPanel = null;

    [ClientRpc]
    public void RpcShowInGamePlayersScore() =>
        _inGamePlayersScore = new InGamePlayersScore(_serverRoomManager.Players);

    [ClientRpc]
    public void RpcHideInGamePlayersScore() =>
        _inGamePlayersScore = null;

    [ClientRpc]
    public void RpcShowRoomGUI() =>
        _roomManager.showRoomGUI = true;

    [ClientRpc]
    public void RpcHideRoomGUI() =>
        _roomManager.showRoomGUI = false; //mb we don't need it?

    [Server]
    public void ShowStartGameButton(Action onClick) =>
        _startGameButton = new StartGameButton(onClick);

    [Server]
    public void HideStartGameButton() =>
        _startGameButton = null;
}
